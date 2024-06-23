using System.Collections;
using System.Collections.Generic;

using UnityEngine;  // Sử dụng namespace UnityEngine để có thể sử dụng các lớp và phương thức của Unity

[CreateAssetMenu]  // Thuộc tính để tạo menu trong Unity Editor để tạo mới đối tượng ScriptableObject từ context menu
public class CharacterDatabase : ScriptableObject  // Định nghĩa lớp CharacterDatabase kế thừa từ ScriptableObject, để lưu trữ danh sách các nhân vật
{
    public Character[] character;  // Mảng các đối tượng Character để lưu trữ danh sách các nhân vật

    public int CharacterCount  // Thuộc tính (property) trả về số lượng nhân vật trong danh sách
    {
        get
        {
            return character.Length;  // Trả về độ dài của mảng character, tức là số lượng nhân vật trong danh sách
        }
    }

    public Character GetCharacter(int index)  // Phương thức để lấy thông tin của nhân vật tại vị trí index trong danh sách
    {
        return character[index];  // Trả về nhân vật tại vị trí index trong mảng character
    }
}
