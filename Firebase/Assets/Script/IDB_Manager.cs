using Firebase;
using Firebase.Database;
using System;
using System.Collections;

public interface IDB_Manager
{
    void IsSetDBUrl(string dbUrl);

    void Save(FirebaseData firebaseData);

    void Load(string referencename, Action<FirebaseData> onLoaded);
     class FirebaseData
    {
        //EX) 
        public string id;
        public string pw;

        public FirebaseData(string id, string pw)
        {
            this.id = id;
            this.pw = pw;
        }
    }
}

