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
                    MessageBox.Show("�߸��� ��� �Է�", "����");
                    return;

                }
                if(Nth <= 0 || Nth > 100)
                {
                    MessageBox.Show("����� 1~100 ���̿��� �մϴ�", "����");
                    return;
                }
                Url = String.Format("https://api.lorrgs.io/api/spec_ranking/{0}/{1}?difficulty={2}&metric={3}&limit={4}", Spec, Boss, Difficulty, Order, Nth);
                string JsonStr = string.Empty;



                /*Json ��������*/
                JsonStr = Handler.HttpRequest(Url);

                Root rootObj = JsonSerializer.Deserialize<Root>(JsonStr);//������ȭ ����

                if (rootObj.fights.Count == 0)
                {
                    MessageBox.Show("���� �α׸� ã�� �� �����ϴ�!", "����");
                    return;
                }

                textBox2.Text += "Lorrgs ��� WCL �α� MRT �޸� �����\r\n";
                textBox2.Text += "Lorrgs ��ó : " + Url + "\r\n";
                textBox2.Text += "WCL ����Ʈ �ּ� : " + "https://warcraftlogs.com/reports/" + rootObj.fights[Nth - 1].report_id + "\r\n";
                textBox2.Text += "���� : " + rootObj.fights[Nth-1].players[0].name + "\r\n";
                textBox2.Text += "�α�ID : " + rootObj.fights[Nth - 1].report_id + "\r\n";

                textBox2.Text += string.Format("ų Ÿ�� : " + Handler.ConvertMilliseconds(rootObj.fights[Nth - 1].duration) + "\r\n");

                textBox1.Text += "���� : " + Boss + "\r\n";
                textBox1.Text += string.Format("ų Ÿ�� : " + Handler.ConvertMilliseconds(rootObj.fights[Nth - 1].duration) + "\r\n");

                List<Cast> Casts = rootObj.fights[Nth - 1].players[0].casts;
                List<Cast> BCasts = rootObj.fights[Nth - 1].boss.casts;

                int PI = 0, KE = 0, KS = 0, FG = 0, BF = 0, FM = 0;
                int buff = 0;
                foreach(Cast cast in Casts)
                {
                    if(cast.id == 338525)//������ȭ
                    {
                        KE++;
                        buff++;
                    }
                    else if(cast.id == 338525)//�߻��� ��ȥ
                    {
                        KS++;
                        buff++;
                    }
                    else if (cast.id == 10060)//���� ����
                    {
                        PI++;
                        buff++;
                    }
                    else if (cast.id == 327661)//���� ��ȣ��
                    {
                        FG++;
                        buff++;
                    }
                    else if (cast.id == 327710)//���� ��ȣ��
                    {
                        BF++;
                        buff++;
                    }
                    else if (cast.id == 327710)//���� ������
                    {
                        FM++;
                        buff++;
                    }
                }

                textBox2.Text += String.Format("\r\n===���� ���(���� ���� ����)===\r\n");
                textBox2.Text += String.Format("���� ��ȭ : {0}ȸ\r\n", KE);
                textBox2.Text += String.Format("�߻� ��ȥ : {0}ȸ\r\n", KS);
                textBox2.Text += String.Format("���� ���� : {0}ȸ\r\n", PI);
                textBox2.Text += String.Format("���̼�ȣ�� : {0}ȸ\r\n", FG);
                textBox2.Text += String.Format("�ں��� �� : {0}ȸ\r\n", BF);
                textBox2.Text += String.Format("���� ������ : {0}ȸ\r\n",FM);
                textBox2.Text += String.Format("\r\n\r\n�� ����: {0}ȸ\r\n", buff);
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
                MessageBox.Show("���� �߻� : " + ex.Message, "����");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string Url = "https://raid.subcreation.net/";
            string Class = "";
            string Boss = "";
            if(listBox2.SelectedIndex == -1 || listBox1.SelectedIndex == -1 || listBox4.SelectedIndex == -1)
            {
                throw new Exception("���� ����");
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


            if (listBox2.SelectedIndex < 10) //�������� ��ä
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