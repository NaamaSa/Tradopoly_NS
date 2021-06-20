
import socket
import sys
import sqlite3
from threading import Thread
import time
from sqlite3 import Error

server_socket = socket.socket()
server_socket.bind(('0.0.0.0', 8000))
server_socket.listen(5)
clients_sockets = []
messages_to_send = []
client_list = []
admin_connected = False
two_players_list = [[None, None]]
three_players_list = [[None, None, None]]
four_players_list = [[None, None, None, None]]


# creates conncetion with sqlite
def create_connection(db_file):
    conn = None
    try:
        conn = sqlite3.connect(db_file)
        return conn
    except Exception as e:
        print(e)
    return conn


# signs up user
def signup(info_list, client_socket):
    try:
        conn = create_connection(r"C:\Users\Park-Hamada Student\Desktop\Tradopoly_NS\users.db")
        c = conn.cursor()
        email = info_list[1]
        username = info_list[2]
        password = info_list[3]
        c.execute(f"SELECT * FROM users WHERE email = '{email}'")
        conn.commit()
        exists = len(c.fetchall()) > 0
        if not exists:
            c.execute(f"INSERT INTO users VALUES('{email}', '{username}', '{password}' )")
            conn.commit()
            time.sleep(0.5)
            client_socket.send("success".encode())
        else:
            time.sleep(0.5)
            client_socket.send("fail-email".encode())
    except Error as e:
        print(e)

def client_game(list, c):
    for game in list:
        for client in game:
            if client == c:
                return game


# sends messages to all clients
def send_waiting_message(message, num, client_socket):
    print(message + "," + num)
    if num == "2":
        game = client_game(two_players_list,client_socket)
        for client in game:
            client.send(message.encode())
    elif num == "3":
        game = client_game(three_players_list, client_socket)
        for client in game:
            client.send(message.encode())
    elif num == "4":
        game = client_game(four_players_list, client_socket)
        for client in game:
            client.send(message.encode())


# function checks if user info is in database. if yes - returns 'success',
# if no - returns 'fail - mail/password is incorrect'
def check_signin(info_list, client_socket):
    conn = create_connection(r"C:\Users\Park-Hamada Student\Desktop\Tradopoly_NS\users.db")
    c = conn.cursor()
    email = info_list[1]
    password = info_list[2]
    sql = f"SELECT * FROM users WHERE email='{email}' AND password = '{password}'"
    c.execute(sql)
    conn.commit()
    data = c.fetchall()
    if len(data) > 0:
        client_socket.send("success".encode())
    else:
        client_socket.send("fail".encode())


# client quits
def client_quit(current_socket):
    clients_sockets.remove(current_socket)
    print("Connection with client closed")
    for c in client_list:
        if c.get_conn() is current_socket:
            if c.get_is_manager():
                string = "code:2,@{}".format(c.get_name())
            else:
                string = "code:2,{}".format(c.get_name())
            send_waiting_message(string)
            client_list.remove(c)


# check the type of message
def check_message(client_socket, address):
    while True:
        try:
            data = client_socket.recv(1024).decode()
            print(data)
            info_list = data.split(",")
            if info_list[0] == "signup":
                signup(info_list, client_socket)
            elif info_list[0] == "signin":
                check_signin(info_list, client_socket)
            elif info_list[0].lower() == "quit":  # client quits chats
                client_quit(client_socket)
            elif info_list[0].lower() == "movement":  # client moves
                send_waiting_message(data, info_list[4], client_socket)
            elif info_list[0].lower() == "property":  # client does something to property
                send_waiting_message(data, info_list[4], client_socket)
            elif info_list[0].lower() == "money":  # client send money to another
                send_waiting_message(data, info_list[4], client_socket)
            elif info_list[0].lower() == "join_2":  # client choose game with 2 players
                game = two_players_list[get_searching_game(two_players_list)]
                for client in game:
                    if client is None:
                        game[game.index(client)] = client_socket
                        print(f"game:{game}")
                        if None in game:
                            client_socket.send("Not Full".encode())
                        else:
                            two_players_list.insert(len(two_players_list), [None, None])
                            for player in game:
                                player.send(f"Full,{game.index(player)}".encode())
                        break
            elif info_list[0].lower() == "join_3":  # client choose game with 3 players
                game = three_players_list[get_searching_game(three_players_list)]
                for client in game:
                    if client is None:
                        client = client_socket
                        if None in game:
                            client_socket.send("Not Full".encode())
                        else:
                            print("a")
                            three_players_list.insert(len(three_players_list), (None, None, None))
                            for player in game:
                                player.send(f"Full,{game.index(player)}".encode())
            elif info_list[0].lower() == "join_4":  # client choose game with 4 players
                game = four_players_list[get_searching_game(four_players_list)]
                for client in game:
                    if client is None:
                        client = client_socket
                        if None in game:
                            client_socket.send("Not Full".encode())
                        else:
                            four_players_list.insert(len(four_players_list), (None, None, None, None))
                            for player in game:
                                player.send(f"Full,{game.index(player)}".encode())
            elif info_list[0].lower() == "lose":
                print("lose")
        except:
            pass


def get_searching_game(list):
    for tup in list:
        if None in tup:
            return list.index(tup)


def print_users():
    conn = create_connection(r"C:\Users\Park-Hamada Student\Desktop\Tradopoly_NS\users.db")
    c = conn.cursor()
    print(c.execute(
        f"SELECT * FROM users").fetchall())
    conn.commit()


def main():
    while True:
        client_socket, address = server_socket.accept()
        print("new client" + address[0])
        clients_sockets.append(client_socket)
        thread = Thread(target=check_message, args=(client_socket, address,))
        thread.start()


def create_table(conn, str):
    try:
        c=conn.cursor()
        c.execute(str)
    except Error as e:
        print(e)


if __name__ == '__main__':
    conn = create_connection(r"C:\Users\Park-Hamada Student\Desktop\Tradopoly_NS\users.db")
    str = "CREATE TABLE IF NOT EXISTS users(email text PRIMARY KEY, username text NOT NULL, password text NOT NULL)"
    create_table(conn, str)
    main()
