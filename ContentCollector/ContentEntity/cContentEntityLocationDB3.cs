using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Xml;

namespace ContentCollector
{
    public class cContentEntityLocationDB3 : cContentEntitySimple
    {
        public cContentEntityLocationDB3(string name, cContentEntitySimple parent) : base(name, parent) { }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public string FileName { get { return null; } }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            // Запускаем конвертер физики
            Utils.RunProcess(build.GetManglePath(@"bin\win32\db3converter.exe"), build.GetManglePath(Name));
            build.AddContentEntity(new cContentEntitySimple(@"data\world\" + Name + ".p", this));
            build.AddContentEntity(new cContentEntitySimple(@"data\world\" + Name + ".g", this));

            // 
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + build.GetManglePath(Name));
            try
            {
                conn.Open();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }

            #region Graphics
            if (conn.State == ConnectionState.Open)
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "SELECT Graphics FROM _Entities";
                
                SQLiteDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    string name = @"export\gfxlib\" + r["Graphics"].ToString() + ".n2";
                    build.AddContentEntity(new cContentEntityN2(name, this));
                }
                r.Close();
            }
            #endregion

            #region Physics
            if (conn.State == ConnectionState.Open)
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "SELECT Physics FROM _Entities";

                SQLiteDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    string physics = r["Physics"].ToString();
                    if (physics != "" && physics != "static")
                    {
                        string name = @"data\physics\" + physics;
                        build.AddContentEntity(new cContentEntityPhysicsIni(name, this));   
                    }
                }
                r.Close();
            }
            #endregion

            #region ParkingCars
            if (conn.State == ConnectionState.Open)
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "SELECT Physics FROM _Entities WHERE _Category = \"ParkingCar\"";

                SQLiteDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    string name = r["Physics"].ToString().Replace("/p_parking_setup.ini","").Replace("cars/","");
                    build.AddContentEntity(new cContentEntityParkingCar(name, this, name));
                }
                r.Close();
            }
            #endregion

            conn.Dispose();

            AddEntityVariants(build);
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
