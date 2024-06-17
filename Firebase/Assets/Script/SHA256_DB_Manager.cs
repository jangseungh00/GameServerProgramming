using Firebase;
using Firebase.Database;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static IDB_Manager;

public static class CLASS_SHA_256
{
    public static string CalculateSHA256(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);

            StringBuilder stringBuilder = new StringBuilder();

            foreach (byte b in hashBytes)
            {
                stringBuilder.Append(b.ToString("x2"));
            }
            return stringBuilder.ToString();
        }
    }
}

public class SHA256_DB_Manager : MonoBehaviour, IDB_Manager
{
    private readonly string _dbString = "https://ar-project-a3a6f-default-rtdb.asia-southeast1.firebasedatabase.app/";
    private bool _isSearch = false;
    FirebaseData firebaseData;
    object mx = new object();

    
    
    private void OnEnable()
    {
        UI_Login.OnJoinButtonClick += HandleJoinButtonClick;
        UI_Login.OnLoginButtonClick += HandleLoginButtonClick;
    }

    private void OnDisable()
    {
        UI_Login.OnJoinButtonClick -= HandleJoinButtonClick;
        UI_Login.OnLoginButtonClick -= HandleLoginButtonClick;
    }

    void Start()
    {
        IsSetDBUrl(_dbString);
        Debug.Log("DB URL 설정 완료: " + _dbString);


        lock(mx)
        {

        }
    }

    private void HandleJoinButtonClick(string id, string pw)
    {
        SetFirebaseData(id, pw);
        Save(firebaseData);
    }

    private void HandleLoginButtonClick(string id, string pw)
    {
        SetFirebaseData(id, pw);
        Load(id, (loadedData) =>
        {
            if (loadedData != null)
            {
                Debug.Log($"{loadedData.pw}, {id}");
                if(firebaseData.pw == loadedData.pw)
                {
                    Debug.Log("로그인 성공");
                }
            }
            else
            {
                Debug.Log("로그인 실패: 데이터가 없습니다.");
            }
        });

    }

    public FirebaseData SetFirebaseData(string id, string pw)
    {
        string hashedPw = CLASS_SHA_256.CalculateSHA256(pw);
        firebaseData = new FirebaseData(id, hashedPw);
        return firebaseData;
    }

    public void IsSetDBUrl(string dbUrl)
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new Uri(_dbString);
    }

    public void Save(IDB_Manager.FirebaseData firebaseData)
    {
        string jsonData = JsonUtility.ToJson(firebaseData);
        DatabaseReference databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        databaseReference.Child(firebaseData.id).SetRawJsonValueAsync(jsonData);
    }

    public void Load(string id, Action<FirebaseData> onLoaded)
    {
        DatabaseReference databaseReference = FirebaseDatabase.DefaultInstance.GetReference(id);
        _isSearch = true;
        databaseReference.GetValueAsync().ContinueWith(task =>
        {
            FirebaseData loadedData = null;

            if (task.IsFaulted)
            {
                Debug.LogError($"DB에서 데이터를 가져오는데 실패했습니다. {task.Exception.Message}");
            }
            else if (task.IsCanceled)
            {
                Debug.Log("DB에서 데이터를 가져오는걸 취소했습니다.");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot dataSnapshot = task.Result;
                string jsonData = dataSnapshot.GetRawJsonValue();
                if (!string.IsNullOrEmpty(jsonData))
                {
                    loadedData = JsonUtility.FromJson<IDB_Manager.FirebaseData>(jsonData);
                }
            }
            _isSearch = false;
            onLoaded?.Invoke(loadedData);
        });
    }
}