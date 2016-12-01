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
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public override void Parse(cBuild build)
        {
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + this.FileName);
            try
            {
                conn.Open();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (conn.State == ConnectionState.Open)
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "SELECT Graphics FROM _Entities";
                
                SQLiteDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    string name = @"export/gfxlib/" + r["Graphics"].ToString() + ".n2";
                    build.AddContentEntity(typeof(cContentEntityN2), name, name, this);
                }
                r.Close();
            }

            #region ParkingCars
            if (conn.State == ConnectionState.Open)
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                cmd.CommandText = "SELECT Physics FROM _Entities WHERE _Category = \"ParkingCar\"";

                SQLiteDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    string name = "ParkingCar:" + r["Physics"].ToString().Replace("/p_parking_setup.ini","");
                    build.AddContentEntity(typeof(cContentEntityParkingCar), name, null, this);
                }
                r.Close();
            }
            #endregion

            conn.Dispose();
        }
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }   // cContentEntityMission
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}   // сContentCollector
