using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerGradeList", menuName = "ScriptableObjects/PlayerGradeList", order = 1)]
public class PlayerGradeList : ScriptableObject
{
    public List<PlayerGrade> dataList = new List<PlayerGrade>();
}
