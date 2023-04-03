using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FollowMouse : MonoBehaviour {
    [Header("Properties")]
    [SerializeField] private Vector3 _offset = Vector3.zero;

    #region Properties
    public Vector3 Offset {
        get => _offset;
        set => _offset = value;
    }
    #endregion

    #region Unity
    private void Update ( ) {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + Offset;
        transform.position = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, transform.position.z);
    }
    #endregion
}
