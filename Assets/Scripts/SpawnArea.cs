// using UnityEngine;

// public class SpawnArea : MonoBehaviour
// {
//     [Header("Classic Spawn Area")]
//     [SerializeField] private Vector2 Classic_RectSize;
//     [SerializeField] private Vector2 Classic_Position;

//     [Header("Rain Spawn Area")]
//     [SerializeField] private Vector2 Rain_RectSize;
//     [SerializeField] private Vector2 Rain_Position;

//     [Header("Flick Spawn Area")]
//     [SerializeField] private Vector2 Flick_RectSize;
//     [SerializeField] private Vector2 Flick_Position;

//     private RectTransform rect;

//     void Start()
//     {
//         rect = GetComponent<RectTransform>();
//     }

//     public void UpdateSpawnArea(GameMode mode)
//     {
//         Debug.Log(rect.sizeDelta);
//         Debug.Log(rect.position);
//         switch (mode)
//         {
//             case GameMode.Classic:
//                 {
//                     rect.sizeDelta = Classic_RectSize;
//                     rect.localPosition = Classic_Position;
//                     break;
//                 }
//             case GameMode.Rain:
//                 {
//                     rect.sizeDelta = Rain_RectSize;
//                     rect.localPosition = Rain_Position;
//                     break;
//                 }
//             case GameMode.Flick:
//                 {
//                     rect.sizeDelta = Flick_RectSize;
//                     rect.localPosition = Flick_Position;
//                     break;
//                 }
//         }
//     }
// }
