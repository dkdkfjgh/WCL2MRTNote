#pragma warning disable 0169, 0414, anyothernumber

using System.Text.Json;
using System.Web;
using WCL2MRTNote;
using System.Diagnostics;

namespace WCL2MRTNoteGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            const string Ver = "1.2.0";
            this.Text += "-v" + Ver;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";

            int Nth;

            string Url;
            string Spec;
            string Boss;
            string Order;
            string Difficulty;

            try
            {
                Spec = listBox1.SelectedItem.ToString();
                Boss = listBox2.SelectedItem.ToString();
                Order = listBox3.SelectedItem.ToString();
                Difficulty = listBox4.SelectedItem.ToString();

                if(Int32.TryParse(textBox3.Text, out Nth) == false)
                {
                    MessageBox.Show("잘못된 등수 입력", "오류");
                    return;

                }
                if(Nth <= 0 || Nth > 100)
                {
                    MessageBox.Show("등수는 1~100 사이여야 합니다", "오류");
                    return;
                }
                Url = String.Format("https://api.lorrgs.io/api/spec_ranking/{0}/{1}?difficulty={2}&metric={3}&limit={4}", Spec, Boss, Difficulty, Order, Nth);
                string JsonStr = string.Empty;



                /*Json 가져오기*/
                JsonStr = Handler.HttpRequest(Url);

                Root rootObj = JsonSerializer.Deserialize<Root>(JsonStr);//역직렬화 수행

                if (rootObj.fights.Count == 0)
                {
                    MessageBox.Show("전투 로그를 찾을 수 없습니다!", "오류");
                    return;
                }

                textBox2.Text += "Lorrgs 기반 WCL 로그 MRT 메모 추출기\r\n";
                textBox2.Text += "Lorrgs 출처 : " + Url + "\r\n";
                textBox2.Text += "WCL 리포트 주소 : " + "https://warcraftlogs.com/reports/" + rootObj.fights[Nth - 1].report_id + "\r\n";
                textBox2.Text += "유저 : " + rootObj.fights[Nth-1].players[0].name + "\r\n";
                textBox2.Text += "로그ID : " + rootObj.fights[Nth - 1].report_id + "\r\n";

                textBox2.Text += string.Format("킬 타임 : " + Handler.ConvertMilliseconds(rootObj.fights[Nth - 1].duration) + "\r\n");

                textBox1.Text += "보스 : " + Boss + "\r\n";
                textBox1.Text += string.Format("킬 타임 : " + Handler.ConvertMilliseconds(rootObj.fights[Nth - 1].duration) + "\r\n");

                List<Cast> Casts = rootObj.fights[Nth - 1].players[0].casts;
                List<Cast> BCasts = rootObj.fights[Nth - 1].boss.casts;

                int PI = 0, KE = 0, KS = 0, FG = 0, BF = 0, FM = 0;
                int buff = 0;
                foreach(Cast cast in Casts)
                {
                    if(cast.id == 338525)//동족강화
                    {
                        KE++;
                        buff++;
                    }
                    else if(cast.id == 338525)//야생의 영혼
                    {
                        KS++;
                        buff++;
                    }
                    else if (cast.id == 10060)//마력 주입
                    {
                        PI++;
                        buff++;
                    }
                    else if (cast.id == 327661)//페이 수호자
                    {
                        FG++;
                        buff++;
                    }
                    else if (cast.id == 327710)//페이 수호자
                    {
                        BF++;
                        buff++;
                    }
                    else if (cast.id == 327710)//마법 집중점
                    {
                        FM++;
                        buff++;
                    }
                }

                textBox2.Text += String.Format("\r\n===버프 목록(셀프 버프 포함)===\r\n");
                textBox2.Text += String.Format("동족 강화 : {0}회\r\n", KE);
                textBox2.Text += String.Format("야생 영혼 : {0}회\r\n", KS);
                textBox2.Text += String.Format("마력 주입 : {0}회\r\n", PI);
                textBox2.Text += String.Format("페이수호자 : {0}회\r\n", FG);
                textBox2.Text += String.Format("자비의 페어리 : {0}회\r\n", BF);
                textBox2.Text += String.Format("마법 집중점 : {0}회\r\n",FM);
                textBox2.Text += String.Format("\r\n\r\n총 버프: {0}회\r\n", buff);
                textBox2.Text += String.Format("=========================\r\n");

                const int OneCycleMS = 13000;
                const int BossCycleMS = 4000;
                int timestamp;

                if (Casts[0].id != 2825)
                {
                    timestamp = Casts[0].ts + OneCycleMS;
                    textBox1.Text += string.Format("{{time:{0}}}-", Handler.ConvertMilliseconds(Casts[0].ts));
                    if(checkBox1.Checked == true)
                    {
                        foreach (var C2 in BCasts)
                        {
                            if (Math.Abs(C2.ts - Casts[0].ts) < BossCycleMS)
                            {
                                textBox1.Text += string.Format("{0}{{spell:{1}}}-", Handler.SpellID2Name(C2.id), C2.id) + " ";
                                break;
                            }
                        }

                    }

                }
                else
                {
                    timestamp = Casts[1].ts + OneCycleMS;
                    textBox1.Text += string.Format("{{time:{0}}}-", Handler.ConvertMilliseconds(Casts[1].ts));
                    if (checkBox1.Checked == true)
                    {
                        foreach (var C2 in BCasts)
                        {
                            if (Math.Abs(C2.ts - Casts[1].ts) < BossCycleMS)
                            {
                                textBox1.Text += string.Format("{0}{{spell:{1}}}-", Handler.SpellID2Name(C2.id), C2.id) + " ";
                                break;
                            }
                        }
                    }
                }
                foreach (var C in Casts)
                {
                    if (C.id == 2825)
                    {
                        continue;
                    }
                    if (C.ts < timestamp)
                    {
                        textBox1.Text += string.Format("{{spell:{0}}}", C.id) + " ";
                    }
                    else
                    {
                        textBox1.Text += "\r\n";
                        textBox1.Text += string.Format("{{time:{0}}}-", Handler.ConvertMilliseconds(C.ts));
                        if (checkBox1.Checked == true)
                        {
                            foreach (var C2 in BCasts)
                            {
                                if (Math.Abs(C2.ts - C.ts) < BossCycleMS)
                                {
                                    textBox1.Text += string.Format("{0}{{spell:{1}}}-", Handler.SpellID2Name(C2.id), C2.id) + " ";
                                    break;
                                }
                            }
                        }
                        textBox1.Text += string.Format("{{spell:{0}}}", C.id) + " ";
                        timestamp = C.ts + OneCycleMS;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("오류 발생 : " + ex.Message, "오류");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string Url = "https://raid.subcreation.net/";
            string Class = "";
            string Boss = "";
            if(listBox2.SelectedIndex == -1 || listBox1.SelectedIndex == -1 || listBox4.SelectedIndex == -1)
            {
                throw new Exception("선택 오류");
            }
            Class = ReverseWords(listBox1.SelectedItem.ToString());
            Boss = listBox2.SelectedItem.ToString();

            if (Class.Contains("beastmastery"))
            {
                Class = Class.Replace("beastmastery", "beast-mastery");
            }
            else if (Class.Contains("demonhunter"))
            {
                Class = Class.Replace("demonhunter", "demon-hunter");
            }
            else if (Class.Contains("deathknight"))
            {
                Class = Class.Replace("deathknight", "death-knight");
            }


            if (listBox2.SelectedIndex < 10) //나스리아 성채
            {
                Url += Class + "-" + Boss;
            }
            else if (listBox2.SelectedIndex < 20)
            {
                    
                if (Boss.Contains("tarragrue"))
                {
                    Boss = "tarragrue";
                }
                else if (Boss.Contains("eye"))
                {
                    Boss = "eye-of-the-jailer";
                }
                else if (Boss.Contains("fatescribe"))
                {
                    Boss = "fatescribe-roh-kalo";
                }
                Url += "sanctum-" + Class + "-" + Boss;
            }
            else
            {
                if (Boss.Contains("zovaal"))
                {
                    Boss = "the-jailer";
                }
                Url += "sepulcher-" + Class + "-" + Boss;
            }
            if(listBox4.SelectedItem.ToString() == "heroic")
            {
                Url += "-heroic";
            }
            Url += ".html";

            Url = Url.Replace("-cn", "");

            Process.Start(new ProcessStartInfo(Url) { UseShellExecute = true });
        }

        private string ReverseWords(string s)
        {
            string result = "";
            List<string> wordsList = new List<string>();
            string[] words = s.Split(new[] {"-"}, StringSplitOptions.None);
            for (int i = words.Length - 1; i >= 0; i--)
            {
                result += words[i] + "-";
            }
            wordsList.Add(result);
            result = "";
            foreach (String str in wordsList)
            {

                result += str;
            }
            result = result.Remove(result.Length - 1);
            return result;
        }
    }
}