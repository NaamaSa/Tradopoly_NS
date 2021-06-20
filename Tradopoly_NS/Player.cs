using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tradopoly_NS
{
    public class Player
    {
        private string color;
        private int num;
        private int[] moneyList; // [500,100,50,20,10,5,1]
        private ArrayList cardsLists; // StreetArr, trainArr, companyArr, specialCardList
        private int totalMoney;
        private int x;
        private int y;

        public Player(string color, int num, int[] moneyList, string[] streetCardArr, string[] trainCardArr, string[] companyCardArr, string[] specialCardArr, int totalMoney, int x, int y)
        {
            this.color = color;
            this.num = num;
            this.moneyList = moneyList;
            this.cardsLists = new ArrayList();
            this.cardsLists.Add(streetCardArr);
            this.cardsLists.Add(trainCardArr);
            this.cardsLists.Add(companyCardArr);
            this.cardsLists.Add(specialCardArr);
            this.totalMoney = totalMoney;
            this.x = x;
            this.y = y;
        }

        //get
        public string GetColor() { return this.color; }
        public int GetNum() { return this.num; }
        public int[] GetMoneyList() { return this.moneyList; }
        public ArrayList GetCardsLists() { return this.cardsLists; }
        public int GetTotalMoney() { return this.totalMoney; }
        public int GetX() { return this.x; }
        public int GetY() { return this.y; }
        //set
        public void SetAddMoneyList(int i, int add) { this.moneyList[i] += add; }
        public void SetSubMoneyList(int i, int sub) { this.moneyList[i] -= sub; }
        public void SetAddStreetList(string card) { this.cardsLists.Insert(0, card); }
        public void SetAddTrainList(string card) { this.cardsLists.Insert(1, card); }
        public void SetAddCompanyList(string card) { this.cardsLists.Insert(2, card); }
        public void SetAddSpecialList(string card) { this.cardsLists.Insert(3, card); }
        public void SetSubTotalMoney(int money) { this.totalMoney -= money; }
        public void SetAddTotalMoney(int money) { this.totalMoney += money; }
        public void SetX(int x) { this.x = x; }
        public void SetY(int y) { this.y = y; }
    }
}
