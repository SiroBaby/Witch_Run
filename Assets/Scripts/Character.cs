using System.Collections;
using System.Collections.Generic;

using UnityEngine;  // Sử dụng namespace UnityEngine để có thể sử dụng các lớp và phương thức của Unity

[System.Serializable]  // Đánh dấu lớp này có thể được Serialize, tức là có thể hiển thị các thuộc tính của nó trong Inspector của Unity
public class Character  // Khai báo lớp Character
{

    public Sprite characterSprite;  // Public field để lưu trữ Sprite của nhân vật, cho phép thiết lập từ Inspector của Unity

}
