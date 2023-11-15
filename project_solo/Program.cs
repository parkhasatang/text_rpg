using System.Numerics;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace project_solo
{
    internal class Program
    {

        private static Character player; //이렇게 설정해주면 Program클래스 내의 모든 메서드에서 이 변수에 접근할 수 있다.
        private static List<Item> items; //아이템이 많으니 리스트로 만들어준다.


        static void Main(string[] args)
        {
            /*   1.게임 시작 화면
                   2.상태보기
                   3.인벤토리*/

            GameDataSetting();
            DisplayIntro();
        }

        static void GameDataSetting()
        {
            // 캐릭터 정보 세팅
            player = new Character("Chad", "전사", 1, 10, 5, 100, 1500);

            // 아이템 정보 세팅
            items = new List<Item>(); //리스트 초기화

            //아이템을 추가할려면 아래에 적어준다.
            items.Add(new Item(1, "무딘 검", "날이 많이 무딘 검이다.", 2, 0));
            items.Add(new Item(2, "가죽 갑옷", "가죽으로 만들어진 갑옷이다.", 0, 3));
        }

        static void DisplayIntro()
        {

            Console.Clear();
            
            Console.WriteLine("던전마을에 오신것을 환영합니다.");
            Console.WriteLine("이곳에서 던전에 들어가기 전 행동을 선택하여 주세요.");
            Console.WriteLine();
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine();
            int input = CheckValidInput(1, 2);
            switch (input)
            {
                case 1:
                    DisplayMyInfo();
                    break;

                case 2:
                    // 인벤토리 작업해보기
                    DisplayInventory();
                    break;
            }
        }

        static int CheckValidInput(int min, int max) //입력값이 제대로 된 것인지 확인하는 함수.
        {
            while (true)
            {
                string input = Console.ReadLine();

                bool parseSuccess = int.TryParse(input, out var ret);
                if (parseSuccess)
                {
                    if (ret >= min && ret <= max)
                        return ret;
                }

                Console.WriteLine("잘못된 입력입니다.");
            }
        }

        static void DisplayMyInfo()
        {
            Console.Clear();

            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보를 표시합니다.");
            Console.WriteLine();
            Console.WriteLine($"Lv.{player.Level}");
            Console.WriteLine($"{player.Name}({player.Job})");
            Console.WriteLine($"공격력 :{player.Atk}");
            Console.WriteLine($"방어력 : {player.Def}");
            Console.WriteLine($"체력 : {player.Hp}");
            Console.WriteLine($"Gold : {player.Gold} G");
            Console.WriteLine();
            Console.WriteLine("0. 나가기");

            int input = CheckValidInput(0, 0);
            switch (input)
            {
                case 0:
                    DisplayIntro();
                    break;
            }
        }

        static void DisplayInventory()
        {
            Console.Clear();

            Console.WriteLine("인벤토리-장착관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("[아이템 목록]");
            Console.WriteLine("");
            for (int i = 0; i < items.Count; i++) //모든 아이템들을 보여준다.
            {
                Console.WriteLine($"{i + 1}. {items[i].ItemName}"); //i가 0부터니깐 숫자는 +1을 해서 "1.무딘 검" 이렇게 숫자가 나오게 해준다.
            }

            Console.WriteLine("") ;
            Console.WriteLine("0. 나가기");

            int input = CheckValidInput(0, items.Count);
            switch (input)
            {
                case 0:
                    DisplayIntro();
                    break;
                default:
                    // 선택한 아이템을 착용
                    EquipItemAndDisplayInfo(items[input - 1]);
                    break;
            }

        }
        static void EquipItemAndDisplayInfo(Item selectedItem)
        {
 
            player.EquipItem(selectedItem);


            //EquipItem() 메서드에 메세지를 적어놔서 중복으로 두번 뜨기 때문에 여기에 있는 메세지는 주석으로 처리해 주었습니다.
            /*Console.WriteLine($"{selectedItem.ItemName}을(를) 착용했습니다.");*/
            Console.WriteLine("");
            int input = CheckValidInput(0, items.Count);
            switch (input)
            {
                case 0:
                    DisplayIntro();
                    break;
                default:
                    EquipItemAndDisplayInfo(items[input - 1]);
                    break;
            }


        }
    }

    public class Character
    {

        public Item EquippedItem { get; private set; }

       /* private bool hasEquippedItem;*/ // 아이템 장착을 판별하는 불값 변수를 만들어줍니다.
        public List<Item> EquippedItems { get; private set; }//위에 불값 변수는 아이템을 하나만 착용 할 수 밖에 없어서 리스트로 다시 만들었습니다.

        public string Name { get; }
        public string Job { get; }
        public int Level { get; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public int Hp { get; }
        public int Gold { get; }

        public Character(string name, string job, int level, int atk, int def, int hp, int gold)
        {
            Name = name;
            Job = job;
            Level = level;
            Atk = atk;
            Def = def;
            Hp = hp;
            Gold = gold;


            /*hasEquippedItem = false;*/ // 처음에는 캐릭터가 아무것도 장착하지 않았다고 표시해 줘야 하기 때문에 Character 생성자 안에 써준다.
            EquippedItems = new List<Item>();
        }

        // 아이템 착용 메서드
        public void EquipItem(Item item)
        {
            /*if (!hasEquippedItem)//hasEquippedItem = false;니깐 실행해줍니다.
            {
                EquippedItem = item;
                Atk += item.ItemAtk; // 아이템의 공격력을 추가합니다.
                Def += item.ItemDef; // 아이템의 방어력을 추가합니다.
                hasEquippedItem = true; // 아이템 장착의 유무를 true로 바꿔줍니다.
                                        // 아이템 식별자를 통해 착용된 아이템에 따라 처리
                switch (item.ItemId)//if else문 안에 스위치문을 넣어 아이템 ID에 따라 처리를 해주었습니다.
                {
                    case 1:
                        Console.WriteLine($"{item.ItemName}을(를) 착용했습니다.");
                        break;
                    case 2:
                        Console.WriteLine($"{item.ItemName}을(를) 입었습니다.");
                        break;
                        // 다른 아이템에 대한 처리를 추가하고 싶으면 여기에 추가하면 됩니다.
                }
            }
            else
            {
                Console.WriteLine("이미 아이템을 장착하였습니다.");
            }*/
            
            //위에 코드는 아이템을 하나만 장착할 수 있기 때문에 아래 if문으로 다시 만들어 줬습니다.

            // 이미 착용한 아이템인지 확인
            if (EquippedItems.Contains(item))//리스트에서 Contains 메서드를 사용해서 장비를 착용했는지 아닌지를 판별해 줍니다.
            {
                Console.WriteLine($"{item.ItemName}은(는) 이미 착용한 아이템입니다.");
            }
            else
            {
                // 아이템 추가
                EquippedItems.Add(item);
                Atk += item.ItemAtk; // 아이템의 공격력을 추가합니다.
                Def += item.ItemDef; // 아이템의 방어력을 추가합니다.
                Console.WriteLine($"{item.ItemName}을(를) 착용했습니다.");
            }

        }



    }

    public class Item
    {
        public int ItemId { get; } // 검을 끼면 다른 아이템이 안껴져서 아이템 ID를 생성하여 검도 끼고 갑옷도 입을 수 있게 해줍니다.
        public string ItemName { get; }
        public string ItemInfo {  get; }
        public int ItemAtk { get; }
        public int ItemDef { get; }

        public Item(int itemId, string itemName, string itemInfo, int itemAtk, int itemDef)
        {
            ItemId = itemId;
            ItemName = itemName;
            ItemInfo = itemInfo;
            ItemAtk = itemAtk;
            ItemDef = itemDef;
        }
    }
}
