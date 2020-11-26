using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using Newtonsoft.Json;
using UIKit;

namespace IOS_MCL
{
    public static class Globals
    {
        public static DataTypes.PendingRecord ActiveRecord;
        public static DataTypes.Room ActiveRoom;
        public static DataTypes.User ActiveUser;
        public static string MainMenuInfo;
        public static bool SkippedRoomSelect = false;
        public static TaskFactory TaskFactory;
        public static CancellationTokenSource PopulateCTS;
        public static CancellationTokenSource UpdateTempCTS;
        public static CancellationTokenSource UpdateFinalCTS;

        public static string GetDate()
        {
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString();
            string day = DateTime.Now.Day.ToString();

            return  month + "-" + day + "-" + year;
        }

        public static string GetTime(bool isMili)
        {
            string hour = DateTime.Now.Hour.ToString();
            string min = DateTime.Now.Minute.ToString();
            string sec = DateTime.Now.Second.ToString();
            string msec = DateTime.Now.Millisecond.ToString();

            if (isMili)
            {
                return hour + "." + min + "." + sec + "." + msec;
            }
            return hour + "." + min + "." + sec;
        }
        public static class Colors
        {
            public static UIColor White = new UIColor(1, 1, 1, 1);
            public static UIColor Green = new UIColor(0, 1, 0, 1);
            public static UIColor Red = new UIColor(1, 0, 0, 1);
            public static UIColor Backdrop = new UIColor(.3f, .6f, 1, 1);
        }
        public static class DataTypes
        {
            public static UserRoot Users = new UserRoot();
            public static RoomRoot Rooms = new RoomRoot();
            public static PendingRecordRoot Records = new PendingRecordRoot();
            public static bool TempUpdateBusy = false;
            public static List<PendingRecord> TempUpdateQueue = new List<PendingRecord>();
            public static bool FinalUpdateBusy = false;
            public static List<PendingRecord> FinalUpdateQueue = new List<PendingRecord>();
            public static HttpClient client = new HttpClient();

            public class User
            {
                public string uid { get; set; }
                public string fname { get; set; }
                public string lname { get; set; }
                public string fullname { get; set; }
                public string bldg { get; set; }
                public string shift { get; set; }
                public string rooms { get; set; }
                public List<string> RemainingRooms { get; set; }
                public Dictionary<string, List<string>> CompletedRooms { get; set; }
            }

            public class UserRoot
            {
                public List<User> users { get; set; }
            }

            public class Room
            {
                public string rid { get; set; }
                public string tname { get; set; }
                public string data { get; set; }
                public bool hasS { get; set; }
                public bool hasT { get; set; }
                public bool hasFloor { get; set; }
                public bool hasCarpet { get; set; }
                public bool hasBRoom { get; set; }
                public bool hasSani { get; set; }
            }

            public class RoomRoot
            {
                public List<Room> rooms { get; set; }
            }

            public class PendingRecord
            {
                public string userid { get; set; }
                public string date { get; set; }
                public string data { get; set; }
                public string lastroom { get; set; }
                public string lastdata { get; set; }
            }

            public class PendingRecordRoot
            {
                public List<PendingRecord> pending_records { get; set; }
            }

            public static async Task PopulateData()
            {
                List<KeyValuePair<string, string>> userKvp = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("cmd", "get"),
                new KeyValuePair<string, string>("val1", "users"),
                new KeyValuePair<string, string>("val2", "*")
            };
                List<KeyValuePair<string, string>> roomKvp = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("cmd", "get"),
                new KeyValuePair<string, string>("val1", "rooms"),
                new KeyValuePair<string, string>("val2", "*")
            };
                List<KeyValuePair<string, string>> recordKvp = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("cmd", "get"),
                new KeyValuePair<string, string>("val1", "pending_records"),
                new KeyValuePair<string, string>("val2", "*")
            };
                try
                {
                    HttpResponseMessage userResult = await client.PostAsync("http://69.207.170.153:8237/restsrv/RestController.php?", new FormUrlEncodedContent(userKvp));
                    HttpResponseMessage roomResult = await client.PostAsync("http://69.207.170.153:8237/restsrv/RestController.php?", new FormUrlEncodedContent(roomKvp));
                    HttpResponseMessage recResult = await client.PostAsync("http://69.207.170.153:8237/restsrv/RestController.php?", new FormUrlEncodedContent(recordKvp));
                    Users = JsonConvert.DeserializeObject<UserRoot>(userResult.Content.ReadAsStringAsync().Result);
                    Rooms = JsonConvert.DeserializeObject<RoomRoot>(roomResult.Content.ReadAsStringAsync().Result);
                    Records = JsonConvert.DeserializeObject<PendingRecordRoot>(recResult.Content.ReadAsStringAsync().Result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }

                foreach (User user in Users.users)
                {
                    Console.WriteLine("User with ID of: " + user.uid + " added to list!");
                    user.fullname = user.fname + " " + user.lname;
                }
                foreach (Room room in Rooms.rooms)
                {
                    char[] explodeData = room.data.ToCharArray();
                    room.hasS = CharToBool(explodeData[0]);
                    room.hasT = CharToBool(explodeData[1]);
                    room.hasFloor = CharToBool(explodeData[2]);
                    room.hasCarpet = CharToBool(explodeData[3]);
                    room.hasBRoom = CharToBool(explodeData[4]);
                    room.hasSani = CharToBool(explodeData[5]);
                    Console.WriteLine("Room with ID of: " + room.rid + " added to list! bools: " + room.hasS + " " + room.hasT + " " + room.hasFloor + " " + room.hasCarpet + " " + room.hasSani);
                }
                foreach (PendingRecord rec in Records.pending_records)
                {
                    Console.WriteLine("Record with userID of: " + rec.userid + " added to list!");
                }
            }

            public static async Task UpdateTempRecord(PendingRecord record)
            {
                TempUpdateQueue.Add(record);
                List<KeyValuePair<string, string>> recordKvp = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("cmd", "ins"),
                new KeyValuePair<string, string>("val1", "pending_records")
            };
                if (!TempUpdateBusy)
                {
                    if (TempUpdateQueue.Count > 0)
                    {
                        TempUpdateBusy = true;
                        PendingRecord rec = TempUpdateQueue[0];
                        string Keys = "`userid`, `date`, `data`, `lastroom`, `lastdata`";
                        string Vals = "'" + rec.userid + "', '" + rec.date + "', '" + rec.data + "', '" + rec.lastroom + "', '" + rec.lastdata + "'";
                        recordKvp.Add(new KeyValuePair<string, string>("val2", Keys));
                        recordKvp.Add(new KeyValuePair<string, string>("val3", Vals));
                        HttpResponseMessage recResult = await client.PostAsync("http://69.207.170.153:8237/restsrv/RestController.php?", new FormUrlEncodedContent(recordKvp));
                        if (recResult.Content.ReadAsStringAsync().Result == "1")
                        {
                            Console.WriteLine("UpdateTempRecord: HTTP POST replied OK!");
                            TempUpdateQueue.Remove(TempUpdateQueue[0]);
                            if (TempUpdateQueue.Count > 0)
                            {
                                TempUpdateBusy = false;
                                await UpdateTempRecord(TempUpdateQueue[0]);
                                return;
                            }
                            return;
                        }
                        else
                        {
                            Console.WriteLine("ERROR! Globals.DataTypes.UpdateTempRecord: Post response not 1. Response: " + recResult.Content.ReadAsStringAsync().Result);
                            Console.WriteLine("UpdateTempRecord: Delaying 10 seconds before reattempt of HTTP POST...");
                            Thread.Sleep(10000);
                            TempUpdateBusy = false;
                            await UpdateTempRecord(rec);
                        }
                    }
                }
            }

            public static async Task FinalizeRecord(PendingRecord record)
            {
                FinalUpdateQueue.Add(record);
                List<KeyValuePair<string, string>> recordKvp = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("cmd", "ins"),
                    new KeyValuePair<string, string>("val1", "pending_records")
                };

                if (FinalUpdateQueue.Count > 0 && !TempUpdateBusy && TempUpdateQueue.Count == 0)
                {
                    FinalUpdateBusy = true;
                    PendingRecord rec = FinalUpdateQueue[0];
                    ActiveUser.CompletedRooms[rec.lastroom] = new List<string> { rec.lastroom, GetTime(false), rec.lastdata };
                    string Keys = "`userid`, `date`, `data`, `lastroom`, `lastdata`";
                    string Vals = "'" + rec.userid + "', '" + rec.date + "', '" + ConcatRecords() + "', 'NA', 'NA'";
                    recordKvp.Add(new KeyValuePair<string, string>("val2", Keys));
                    recordKvp.Add(new KeyValuePair<string, string>("val3", Vals));
                    HttpResponseMessage recResult = await client.PostAsync("http://69.207.170.153:8237/restsrv/RestController.php?", new FormUrlEncodedContent(recordKvp));
                    if (recResult.Content.ReadAsStringAsync().Result == "1")
                    {
                        Console.WriteLine("FinalizeRecord: HTTP POST replied OK!");
                        ActiveUser.RemainingRooms.Remove(rec.lastroom);
                        rec.lastroom = "NA";
                        rec.lastdata = "NA";
                        ActiveRecord = rec;
                        FinalUpdateQueue.Remove(FinalUpdateQueue[0]);
                        if (FinalUpdateQueue.Count > 0)
                        {
                            FinalUpdateBusy = false;
                            await FinalizeRecord(FinalUpdateQueue[0]);
                            return;
                        }
                        return;
                    }
                    else
                    {
                        Console.WriteLine("ERROR! Globals.DataTypes.FinalizeRecord: Post response not 1. Response: " + recResult.Content.ReadAsStringAsync().Result);
                        Console.WriteLine("FinalizeRecord: Delaying 10 seconds before reattempt of HTTP POST...");
                        Thread.Sleep(10000);
                        FinalUpdateBusy = false;
                        await FinalizeRecord(rec);
                        return;
                    }
                }
                return;
            }
        }

        public static string ConcatRecords()
        {
            string outstr = "";
            if(ActiveUser.CompletedRooms.Count == 0)
            {
                return "0";
            }
            foreach(KeyValuePair<string, List<string>> room in ActiveUser.CompletedRooms)
            {
                if(outstr == "")
                {
                    outstr = room.Value[0] + ";" + room.Value[1] + ";" + room.Value[2];
                }
                else
                {
                    outstr += ">"+room.Value[0] + ";" + room.Value[1] + ";" + room.Value[2];
                }
            }
            return outstr;
        }

        public static bool CharToBool (char input)
        {
            return input == '1';
        }

        private static int BinarySearchForFontSizeForText(NSString text, int minFontSize, int maxFontSize, CGSize size)
        {
            if (maxFontSize < minFontSize)
                return minFontSize;

            int fontSize = (minFontSize + maxFontSize) / 2;
            UIFont font = UIFont.BoldSystemFontOfSize(fontSize);

            CGSize labelSize = UIStringDrawing.StringSize(text, font);

            if (labelSize.Height >= size.Height + 10 && labelSize.Width >= size.Width + 10 && labelSize.Height <= size.Height && labelSize.Width + 10 <= size.Width)
                return fontSize;
            else if (labelSize.Height > size.Height || labelSize.Width > size.Width)
                return BinarySearchForFontSizeForText(text, minFontSize, fontSize - 1, size);
            else
                return BinarySearchForFontSizeForText(text, fontSize + 1, maxFontSize, size);
        }

        public static UIFont SizeLabelToRect(object uiObj)
        {
            int maxFontSize = 60;
            int minFontSize = 5;
            if (uiObj.GetType() == typeof(UILabel))
            {
                UILabel lbl = (UILabel)uiObj;
                int size = BinarySearchForFontSizeForText(new NSString(lbl.Text), minFontSize, maxFontSize, lbl.Frame.Size);

                return lbl.Font.WithSize(size);
            }
            if (uiObj.GetType() == typeof(UIButton))
            {
                UIButton lbl = (UIButton)uiObj;
                int size = BinarySearchForFontSizeForText(new NSString(lbl.TitleLabel.Text), minFontSize, maxFontSize, lbl.Frame.Size);

                return lbl.Font.WithSize(size);
            }
            return null;
        }
    }
}