using System.Linq;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TextRPG
{
    internal class Program
    {
        #region Variable 

        #region Class Name
        enum ClassName
        {
            None = 0,
            Warrior = 1,
            Wizard = 2,
            Rogue =3,
        }

        static string GetClassNameToKor(ClassName className) => className switch
        {
            ClassName.None => "없음",
            ClassName.Warrior => "전사",
            ClassName.Wizard => "마법사",
            ClassName.Rogue => "도적",
            _ => "",
        };
        #endregion

        #region Player
        class Player
        {
            public int Level;
            public string? Name;
            public ClassName Class;
            public int Atk, Def, Hp, Gold;
            public List<Item> Inventory = new();

            public void SetStat(Stat stat, int value)
            {
                switch (stat)
                {
                    case Stat.ATK: Atk += value; break;
                    case Stat.DEF: Def += value; break;
                    case Stat.HP: Hp += value; break;
                }
            }

            public int GetStat(Stat stat) => stat switch
            {
                Stat.ATK => Atk,
                Stat.DEF => Def,
                Stat.HP => Hp,
                _ => 0
            };
        }

        static Player player = new Player()
        {
            Level = 1,
            Name = "",
            Class = ClassName.None,
            Atk = (int)Stat.ATK,
            Def = (int)Stat.DEF,
            Hp = (int)Stat.HP,
            Gold = 1500,
        };
        #endregion

        #region Stat
        enum Stat
        {
            // 기본 스탯
            ATK = 10,
            DEF = 5,
            HP = 100,
        }

        static string GetStatToKor(Stat stat) => stat switch
        {
            Stat.ATK => "공격력",
            Stat.DEF => "방어력",
            Stat.HP => "체력",
            _ => "",
        };
        #endregion

        #region Item
        class Item
        {
            public string ItemName;
            public Stat StatType;
            public int StatValue;
            public string Description;
            public bool IsEquipped;
            public int Price;

            public Item(string itemName, Stat statType, int statValue, string description, bool isEquipped, int price)
            {
                ItemName = itemName;
                StatType = statType;
                StatValue = statValue;
                Description = description;
                IsEquipped = isEquipped;
                Price = price;
            }
        }

        static List<Item> itemList = new List<Item>()
        {
            new Item("수련자 갑옷", Stat.DEF, 5, "수련에 도움을 주는 갑옷입니다.", false, 1000),
            new Item("무쇠 갑옷", Stat.DEF, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", false, 1800),
            new Item("스파르타의 갑옷", Stat.DEF, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", false, 3500),
            new Item("낡은 검", Stat.ATK, 2, "쉽게 볼 수 있는 낡은 검입니다.", false, 600),
            new Item("청동 도끼", Stat.ATK, 5, "어디선가 사용됐던 것 같은 도끼입니다.", false, 1500),
            new Item("스파르타의 창", Stat.ATK, 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", false, 3000),
        };
        #endregion

        #endregion

        static void Main(string[] args)
        {
            SettingScene startScene = new SettingScene();
            startScene.SettingName();
        }

        class SettingScene
        {
            public void SettingName()
            {
                Console.Clear();

                // 이름 설정
                Console.WriteLine(
                    "스파르타 마을에 오신 여러분 환영합니다.\n" +
                    "원하시는 이름을 입력해주세요.\n");

                string input = Console.ReadLine() ?? "";

                Console.WriteLine(
                    "\n" +
                    $"입력하신 이름은 [{input}]입니다.\n");

                // 메뉴 리스트 설정
                var selections = new Dictionary<int, string>
                {
                    {1, "저장"},
                    {2, "취소"}
                };

                // 메뉴 텍스트 표출
                uiHelper.ShowMenuSetting(selections);

                // 유저가 선택한 메뉴 번호에 따른 진행
                switch (uiHelper.GetUserSelection())
                {
                    case 1:
                        player.Name = input;
                        SettingClass();
                        break;
                    case 2:
                    default:
                        SettingName();
                        break;
                }
            }

            public void SettingClass()
            {
                Console.Clear();

                // 직업 설정
                Console.WriteLine(
                    "스파르타 마을에 오신 여러분 환영합니다.\n" +
                    "원하시는 직업을 선택해주세요.\n");

                // 메뉴 리스트 설정
                var selections = new Dictionary<int, string>();

                int index = 1;
                foreach (ClassName className in Enum.GetValues(typeof(ClassName)))
                {
                    if (className == ClassName.None) continue;

                    selections.Add(index++, GetClassNameToKor(className));
                }

                // 메뉴 텍스트 표출
                uiHelper.ShowMenuSetting(selections);

                // 유저가 선택한 메뉴 번호에 따른 진행
                int selection = uiHelper.GetUserSelection();

                if(Enum.IsDefined(typeof(ClassName), selection) && selection != 0)
                {
                    player.Class = (ClassName)selection;
                    MenuScene menuScene = new MenuScene();
                    menuScene.SelectMenu();
                }
                else { SettingClass(); }
            }
        }

        class MenuScene
        {
            #region Menu Select Scene
            public void SelectMenu()
            {
                Console.Clear();

                // 직업 설정
                Console.WriteLine(
                    "스파르타 마을에 오신 여러분 환영합니다.\n" +
                    "이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");


                // 메뉴 리스트 설정
                var selections = new Dictionary<int, string>
                {
                    {1, "상태 보기"},
                    {2, "인벤토리"},
                    {3, "상점"}
                };

                // 메뉴 텍스트 표출
                uiHelper.ShowMenuSetting(selections);

                // 유저가 선택한 메뉴 번호에 따른 진행
                switch (uiHelper.GetUserSelection())
                {
                    case 1: Status(); break;
                    case 2: Inventory(); break;
                    case 3: Shop(); break;
                    default: SelectMenu(); break;
                }
            }

            #endregion

            #region Menu : Status Scene
            void Status()
            {
                Console.Clear();

                Console.WriteLine(
                    "스파르타 마을에 오신 여러분 환영합니다." +
                    "캐릭터의 정보가 표시됩니다.");

                // 상태보기
                Console.WriteLine(
                    $"Lv. {player.Level:D2}\n" +    //레벨
                    $"{player.Name} ( {GetClassNameToKor(player.Class)} )\n" + // 이름 (직업)
                    $"공격력 : {player.Atk} {ShowStatDiff(Stat.ATK)}\n" +  // 공격력
                    $"방어력 : {player.Def} {ShowStatDiff(Stat.DEF)}\n" +  // 방어력
                    $"체력 : {player.Hp} {ShowStatDiff(Stat.HP)}\n" +     // 체력
                    $"Gold : {player.Gold} G\n");  // 골드

                // 나가기 메뉴 표출
                uiHelper.ShowMenuBack();

                // 유저가 선택한 메뉴 번호에 따른 진행
                switch (uiHelper.GetUserSelection())
                {
                    case 0: SelectMenu(); break;
                    default: Status(); break;
                }
            }
            
            int GetBaseStat(Stat stat) { return (int)stat; } // 기본 스탯값 받아오기

            string ShowStatDiff(Stat stat) // 증가한 스탯 값과 그에 따른 텍스트 설정
            {
                int baseValue = GetBaseStat(stat);
                int curValue = stat switch
                {
                    Stat.ATK => player.Atk,
                    Stat.DEF => player.Def,
                    Stat.HP => player.Hp,
                    _ => 0
                };

                int diff = curValue - baseValue;
                return diff == 0 ? "" : $"(+{diff})";
            }

            #endregion

            #region Menu : Inventory Scene
            void Inventory()
            {
                Console.Clear();

                Console.WriteLine(
                    "스파르타 마을에 오신 여러분 환영합니다.\n" +
                    "보유 중인 아이템을 관리할 수 있습니다.\n");

                // 아이템 목록 표출
                uiHelper.ShowItemList(player.Inventory, false, true, false);

                // 메뉴 리스트 설정
                var selections = new Dictionary<int, string>
                {
                    {1, "장착 관리"},
                };

                // 메뉴 텍스트 표출
                uiHelper.ShowMenuSetting(selections);

                // 나가기 메뉴 표출
                uiHelper.ShowMenuBack();

                // 유저가 선택한 메뉴 번호에 따른 진행
                switch (uiHelper.GetUserSelection())
                {
                    case 0: SelectMenu(); break;
                    case 1: Inventory_EquipManagement(); break;
                    default: Inventory(); break;
                }
            }

            void Inventory_EquipManagement()
            {
                Console.Clear();

                Console.WriteLine(
                    "스파르타 마을에 오신 여러분 환영합니다.\n" +
                    "보유 중인 아이템을 관리할 수 있습니다.\n" +
                    "아이템 번호를 입력하여 장착/해제 할 수 있습니다.\n");

                // 보유 중인 아이템 목록 표출
                uiHelper.ShowItemList(player.Inventory, false, true, false);

                // 나가기 메뉴 표출
                uiHelper.ShowMenuBack();

                // 유저가 선택한 메뉴 번호에 따른 진행
                int selection = uiHelper.GetUserSelection();

                if (selection == 0) { Inventory(); }
                else if (selection >= 1 && selection <= player.Inventory.Count)
                {
                    Item selectedItem = player.Inventory[selection - 1];
                    SetEquipItem(selectedItem);
                    Inventory_EquipManagement();
                }
                else { Inventory_EquipManagement(); }
            }

            void SetEquipItem(Item item) // 아이템 장착
            {
                item.IsEquipped = !item.IsEquipped;
                int value = item.StatValue * (item.IsEquipped ? 1 : -1);

                player.SetStat(item.StatType, value);
            }
            #endregion

            #region Menu : Inventory Scene
            void Shop()
            {
                Console.Clear();

                Console.WriteLine(
                    "스파르타 마을에 오신 여러분 환영합니다.\n" +
                    "필요한 아이템을 얻을 수 있는 상점입니다.\n");

                // 아이템 목록 표출
                uiHelper.ShowItemList(itemList, false, false, true);

                // 메뉴 리스트 설정
                var selections = new Dictionary<int, string>
                {
                    {1, "아이템 구매"},
                };

                // 메뉴 텍스트 표출
                uiHelper.ShowMenuSetting(selections);

                // 나가기 메뉴 표출
                uiHelper.ShowMenuBack();

                // 유저가 선택한 메뉴 번호에 따른 진행
                switch (uiHelper.GetUserSelection())
                {
                    case 0: SelectMenu(); break;
                    case 1: Shop_Counter(); break;
                    default: Shop(); break;
                }
            }

            void Shop_Counter()
            {
                Console.Clear();

                Console.WriteLine(
                    "스파르타 마을에 오신 여러분 환영합니다.\n" +
                    "필요한 아이템을 얻을 수 있는 상점입니다.\n" +
                    "아이템의 번호를 입력하여 구매할 수 있습니다.\n");

                // 보유 골드
                Console.WriteLine(
                    "[보유 골드]\n" +
                    $"{player.Gold} G\n");

                // 아이템 목록 표출
                uiHelper.ShowItemList(itemList, true, false, true);

                // 나가기 메뉴 표출
                uiHelper.ShowMenuBack();

                // 유저가 선택한 메뉴 번호에 따른 진행
                int selection = uiHelper.GetUserSelection();
                if(selection == 0) { Shop(); }
                else if(selection > 0) 
                { 
                    Shop();
                    if (!player.Inventory.Contains(itemList[selection - 1]))
                    {
                        if (player.Gold < itemList[selection - 1].Price)
                        {
                            Console.WriteLine("Gold가 부족합니다.");
                        }
                        else
                        {
                            player.Gold -= itemList[selection - 1].Price;
                            Console.WriteLine("구매를 완료했습니다.");
                            player.Inventory.Add(itemList[selection - 1]);
                        }
                    }
                    else { Console.WriteLine("이미 구매한 아이템입니다."); }
                }
                switch (uiHelper.GetUserSelection())
                {
                    case 0: 
                        Console.WriteLine();
                        break;
                    default: Shop_Counter(); break;
                }
            }

            #endregion
        }

        #region UIHelper
        class UIHelper
        {
            public int GetUserSelection()
            {
                int inputNum;

                while (true)
                {
                    Console.Write(
                        "\n" +
                        "원하시는 행동을 숫자만 입력해주세요.\n" +
                        ">>");

                    string input = Console.ReadLine() ?? "";

                    if (int.TryParse(input, out int number))
                    {
                        inputNum = number;
                        Console.Clear();
                        break;
                    }
                    else continue;
                }

                return inputNum;
            }

            public void ShowMenuSetting(Dictionary<int, string> selections)
            {
                // 메뉴 텍스트 표출
                for (int i = 1; i < selections.Count + 1; i++)
                {
                    Console.WriteLine($"[{i}] {selections[i]}\n");
                }
            }

            public void ShowMenuBack()
            {
                Console.WriteLine("[0] 나가기");
            }

            public void ShowItemList(List<Item> items, bool showIndex = false, bool showEquip = false, bool showPurchase = false)
            {
                Console.WriteLine("[아이템 목록]");
                int index = 1;

                foreach (Item item in items)
                {
                    // 아이템 번호
                    if (showIndex) Console.Write($"- {index++} ");
                    else Console.Write(" - ");

                    // 장착 여부
                    if (showEquip && item.IsEquipped) Console.Write("[E] ");
                    else if (showEquip) Console.Write("    ");

                    // 아이템 이름 (최대 10자)
                    Console.Write($"{item.ItemName}");
                    // 글자수에 따른 공백 추가
                    for (int i = 0; i < 10 - item.ItemName.Length; i++) Console.Write(' ');

                    // 스탯 type (최대 3자)
                    string statName = GetStatToKor(item.StatType);
                    Console.Write($" | {statName}");
                    // 글자수에 따른 공백 추가
                    if (statName.Length < 3) Console.Write(' ');

                    // 스탯 value (최대 2자)
                    Console.Write($" +{item.StatValue}");
                    // 글자수에 따른 공백 추가
                    if (item.StatValue < 10) Console.Write(' ');

                    // 설명 (최대 30자)
                    Console.Write($" | {item.Description}");
                    // 글자수에 따른 공백 추가
                    for (int i = 0; i < 30 - item.Description.Length; i++) Console.Write(' ');

                    // 구매 여부 / 가격
                    if (showPurchase)
                    {
                        Console.WriteLine(player.Inventory.Contains(item)
                            ? " | 구매완료"
                            : $" | {item.Price}");
                    }
                    else { Console.WriteLine(); }
                }
            }

        }

        static UIHelper uiHelper = new UIHelper();

        #endregion
    }
}
