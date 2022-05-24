using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    //Toutes les variables accessibles dans l'inspector
    #region Exposed
    public GameObject player;
    [SerializeField] private Vector3 offset = new Vector3(0, 3, -3);

    #endregion

    #region Unity Life Cycle
    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
    #endregion
    //Toutes les fonctions cr��es par l'�quipe
    #region Main Methods

    #endregion

    //Les variables priv�es et prot�g�es
    #region Private & Protected
    #endregion
}
