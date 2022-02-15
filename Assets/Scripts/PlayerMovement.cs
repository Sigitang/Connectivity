using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    Rigidbody2D body;
    public float viewerSpeed = 10f;
    public float BordureEpaisseur = 10f;
    public Vector2 mapLimit; //stock limites de la map en x y
    //public float scrollSpeed = 5f;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        
        if (Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height - BordureEpaisseur)
        {
            pos.y += viewerSpeed * Time.deltaTime;

        }

        if (Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= BordureEpaisseur)
        {
            pos.y -= viewerSpeed * Time.deltaTime;

        }

        if (Input.GetKey(KeyCode.LeftArrow)  || Input.mousePosition.x <= BordureEpaisseur)
        {
            pos.x -= viewerSpeed * Time.deltaTime;

        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width - BordureEpaisseur)
        {
            pos.x += viewerSpeed * Time.deltaTime;

        }

        //Limites de la carte, Mathf.Clamp limite les valeurs à un interval

        pos.x = Mathf.Clamp(pos.x, -mapLimit.x, mapLimit.x);
        pos.y = Mathf.Clamp(pos.y, -mapLimit.y, mapLimit.y);

        //float scroll = Input.GetAxis("Mouse ScrollWheel"); Probleme car sur l'objet joueur et pas sur la camera
        //pos.z += scroll * scrollSpeed * Time.deltaTime;


        transform.position = pos;

        

    }

    private void FixedUpdate()
    {
        

    }
}