using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed;
    public float jumpForce;

    public KeyCode left;
    public KeyCode down;
    public KeyCode right;
    public KeyCode jump;
    public KeyCode throwBall;
    public KeyCode grab;

    // BEGIN DASH
    public float dashDelay;
    public float dashAmmount;
    public float dashPower;
    public float dashCountDown;
    private char direction = 'm';

    private float _dashAmmount;
    private bool _isOnDash;
    private float _dashDelay;
    private float _dashCountDown;
    // END DASH
    
    // BEGIN CROUNCH
    public float crounchSize;
    public float crounchMovimentSpeed;
    public float crounchJumpPower;
    public float crounchDelay;

    private float _moveSpeedBck;
    private float _jumpPowerBackup;
    private float _crounchDelay;

    private bool _isCrounch;
    // END CROUNCH

    //BEGIN DOUBLE JUMP
    private int _doubleJumpState;
    private bool _alreadyJumped;
    // END DOUNBLE JUMP

    private Rigidbody2D theRB;

    public Transform groundCheckPoint;
    public float groundCheckRadius;
    public LayerMask whatIsGround;

    public float rechargeTime;
    private float rechargeTimeCount;

    public GameObject snowBall;
    public Transform throwPoint;
    
    // ITEM
    public GameObject itemCarried;
    private GameObject _onItemObj;
    public GameObject throwableItem;

    public AudioSource throwSound;

    private bool isGrounded;
    
    private Animator anim;

	// Use this for initialization
	void Start () {

        theRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        rechargeTimeCount = rechargeTime;
        _dashDelay = dashDelay;
        _dashAmmount = dashAmmount;
        _isCrounch = false;
        _crounchDelay = 0;

        _moveSpeedBck = moveSpeed;
        _jumpPowerBackup = jumpForce;

        _doubleJumpState = 0;

        _alreadyJumped = false;

    }
	
	// Update is called once per frame
	void Update () {

        _dashCountDown -= Time.deltaTime;
        _dashDelay -= Time.deltaTime;
        _crounchDelay -= Time.deltaTime;
        rechargeTimeCount -= Time.deltaTime; 

        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround);
        if (isGrounded)
        {
            _doubleJumpState = 2;
            _alreadyJumped = false;
        }
        else {
            _doubleJumpState = 1;
        }

        if ( _isOnDash ) {
            //Debug.Log("ta no dash:" + _dashAmmount);
            if (_dashAmmount > 0)
            {
                if( direction == 'l' )
                    theRB.velocity = new Vector2(-dashPower, theRB.velocity.y);
                else
                    theRB.velocity = new Vector2(dashPower, theRB.velocity.y);
                _dashAmmount--;
            }
            else {
                _isOnDash = false;
            }
        }
        else {

            if (Input.GetKeyDown(down))
            {
                if (isGrounded && !_isCrounch)
                {
                    if (_crounchDelay < 0)
                    {
                        // mudar tamanho da imagem
                        transform.localScale = new Vector3(1, crounchSize, 1);
                        // mudar posicao, ajutes da imagem
                        transform.localPosition = new Vector3(transform.localPosition.x, (transform.localPosition.y - ((1 - crounchSize) / 2)), transform.localPosition.z);
                        // mudar velocidade movimento
                        moveSpeed = crounchMovimentSpeed;
                        // mudar potencia do pulo
                        jumpForce = crounchJumpPower;
                        //MUDAR TRHOW POINT
                        _isCrounch = true;
                        _crounchDelay = crounchDelay;

                    }
                    else {
                        Debug.Log("Crounch delay restante:" + _crounchDelay);
                    }
                    
                } else if (_isCrounch) {
                    // mudar tamanho da imagem
                     transform.localScale = new Vector3(1, 1, 1);
                    // mudar tamanho da box de colider -> nao precisa

                    // mudar posicao, ajutes da imagem
                    transform.localPosition = new Vector3(transform.localPosition.x, (transform.localPosition.y + ((1 - crounchSize) / 2)), transform.localPosition.z);
                    // mudar velocidade movimento
                    moveSpeed = _moveSpeedBck;
                    // mudar potencia do pulo
                    jumpForce = _jumpPowerBackup;
                    //MUDAR RETORNAR TRHOW POINT
                    _isCrounch = false;
                }                  
            }
            else if(Input.GetKey(left))
            {
                theRB.velocity = new Vector2(-moveSpeed, theRB.velocity.y);
            }        
            else if (Input.GetKey(right))
            {
                theRB.velocity = new Vector2(moveSpeed, theRB.velocity.y);
            }
            else {
                theRB.velocity = new Vector2(0, theRB.velocity.y);
            }

            if (Input.GetKeyDown(left))
            {
                if (_dashCountDown < 0)
                {
                    _dashCountDown = dashCountDown;
                }
                else
                {
                    if (direction == 'l')
                    {
                        if (_dashDelay < 0 && !_isCrounch)
                        {
                            //dash
                            anim.SetTrigger("Throw");
                            _isOnDash = true;
                            _dashAmmount = dashAmmount;
                            _dashDelay = dashDelay;
                        }
                    }
                }

                direction = 'l';

            }
            else if (Input.GetKeyDown(right))
            {

                if (_dashCountDown < 0)
                {
                    _dashCountDown = dashCountDown;
                }
                else
                {
                    if (direction == 'r')
                    {
                        if (_dashDelay < 0 && !_isCrounch)
                        {
                            //dash
                            anim.SetTrigger("Throw");
                            _isOnDash = true;
                            _dashAmmount = dashAmmount;
                            _dashDelay = dashDelay;
                        }
                    }
                }

                direction = 'r';

            }

            if (Input.GetKeyDown(jump)) {

                // verifica o estado
                // estado 2 -> tava no chao e pode pular duas vezes
                // estado 1 -> pode pular uma vez
                // estado 0 -> não pode pular

                if (_doubleJumpState == 2)
                {
                    theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
                    _doubleJumpState = 1;

                }
                else if (_doubleJumpState == 1 && !_alreadyJumped)
                {
                    theRB.velocity = new Vector2(theRB.velocity.x, jumpForce * 2/3);
                    _alreadyJumped = true;
                }
                else {
                    // nao pode pular -> caso 0
                }
                   
            }

            if (Input.GetKeyDown(grab)) {

                
                SpriteRenderer playerItemSr = itemCarried.gameObject.GetComponent<SpriteRenderer>();

                if( _onItemObj ){
                    
                    SpriteRenderer itemSr = _onItemObj.GetComponent<SpriteRenderer>(); 
                    // ainda nao tem item
                    if( playerItemSr.sprite == null ){
                        playerItemSr.sprite = itemSr.sprite; 

                    } else {
                        //throw
                        GameObject throwableItemClone = (GameObject)Instantiate(throwableItem, throwPoint.position, throwPoint.rotation);
                        throwableItemClone.GetComponent<SpriteRenderer>().sprite = playerItemSr.sprite;
                        if(transform.localScale.x < 0){
                            throwableItemClone.transform.localScale = new Vector3(throwableItemClone.transform.localScale.x*-1,throwableItemClone.transform.localScale.y,throwableItemClone.transform.localScale.z);
                        } 
                        anim.SetTrigger("Throw");
                        throwSound.Play();

                        //grab the other item
                        playerItemSr.sprite = itemSr.sprite;
                    }

                    Destroy(_onItemObj);
                    _onItemObj = null;

                } else {
                    if( playerItemSr.sprite != null ){
                        //throw
                        GameObject throwableItemClone = (GameObject)Instantiate(throwableItem, throwPoint.position, throwPoint.rotation);
                        throwableItemClone.GetComponent<SpriteRenderer>().sprite = playerItemSr.sprite;
                        if(transform.localScale.x < 0){
                            throwableItemClone.transform.localScale = new Vector3(throwableItemClone.transform.localScale.x*-1,throwableItemClone.transform.localScale.y,throwableItemClone.transform.localScale.z);
                        } 
                        anim.SetTrigger("Throw");
                        throwSound.Play();

                        playerItemSr.sprite = null;
                    }

                }
                
            }

            if ( Input.GetKey(throwBall) ) {

                if (rechargeTimeCount < 0) {
                    GameObject ballClone = (GameObject)Instantiate(snowBall, throwPoint.position, throwPoint.rotation);

                    ballClone.transform.localScale = transform.localScale;

                    anim.SetTrigger("Throw");

                    rechargeTimeCount = rechargeTime;

                    throwSound.Play();
                }
           
            }

            if (theRB.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1, transform.localScale.y, 1);
            }
            else if (theRB.velocity.x > 0) {
                transform.localScale = new Vector3(1, transform.localScale.y, 1);
            }
        }

        anim.SetFloat("Speed", Mathf.Abs( theRB.velocity.x ));
        anim.SetBool("Grounded", isGrounded);

    }

    void OnTriggerEnter2D(Collider2D other)
    {        
        if (other.tag == "Item")
        {
            _onItemObj = other.gameObject;

            Debug.Log("Entrou no item;");
            
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {        
        if (other.tag == "Item")
        {
            if (other.gameObject.GetInstanceID() == _onItemObj.GetInstanceID()){
                _onItemObj = null;
                Debug.Log("Saiu do item;");
            }
        }
    }
    

}
