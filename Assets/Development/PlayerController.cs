using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public float speed = 2f;
    public float ySpeedMult = 0.5f; // Multiplier for speed when moving up or down
    public int startHealth = 100;
    public int punchDamage = 10;
    public float iFrameTime = 1.0f;
    public float attackIntervalSeconds = 0.75f;
    private float timeSinceAttack;
    private int currentHealth;
    private Camera camera;
    private PlayerStates state = PlayerStates.idle;
    private float deadTime = 3f;
    private Animator animator;
    private Collider2D hurtbox;
    private Vector3 initScale;
    private LayerMask enemiesMask;
    private float remainingIFrameTime = 0f;
    public int numRaccoons = 0;
    private GameObject HP;
    private GameObject healthBar;
    private GameObject raccoonBar;
    private GameObject RC;

    private enum PlayerStates {
        stopped,
        walking,
        idle,
        attack,
        throwing,
        dying,
        dead
    }

    // Use this for initialization
    void Start() {
        camera = Camera.main;
        currentHealth = startHealth;
        animator = this.GetComponentInChildren<Animator>();
        hurtbox = this.GetComponentInChildren<CircleCollider2D>();
        this.initScale = transform.localScale;
        enemiesMask = ~((1 << 8) | (1 << 9));

        HP = Managers.GetInstance().GetGameProperties().HealthBar;
        healthBar = GameObject.Instantiate(HP);
        Text hpText = healthBar.GetComponentInChildren<Text>();
        hpText.text = currentHealth.ToString();
        this.timeSinceAttack = attackIntervalSeconds;

        RC = Managers.GetInstance().GetGameProperties().RaccoonBar;
        raccoonBar = GameObject.Instantiate(RC);
        Text raccoonText = raccoonBar.GetComponentInChildren<Text>();
        raccoonText.text = numRaccoons.ToString();
    }

    void die() {
        animator.SetBool("dying", true);
        state = PlayerStates.dying;
    }

    public void damage(int amount) {
        if (this.currentHealth < 0) {
            return;
        }
        if (this.remainingIFrameTime > 0 && amount > 0)
        {
            //Debug.Log("player is invincible right now, for " + remainingIFrameTime + " more seconds");
            return;
        }
        if (amount > 0)
        {
            this.remainingIFrameTime = iFrameTime;
        }

        GameObject damageTextPrefab = Managers.GetInstance().GetGameProperties().FloatText;

        GameObject damageText = GameObject.Instantiate(damageTextPrefab);
        damageText.transform.position = transform.position + (Vector3.up * 0.4f);
        FloatTextController cntrl = damageText.GetComponent<FloatTextController>();
        if (amount > 0)
        {
            cntrl.setText(amount.ToString());
            cntrl.setColor(Color.red);
        }
        if (amount < 0)
        {
            cntrl.setText("++" + (amount * -1).ToString());
            cntrl.setColor(Color.cyan);
        }
        
        this.currentHealth -= amount;

        Text hpText = healthBar.GetComponentInChildren<Text>();
        string healthString = currentHealth.ToString();
        if (currentHealth < 0) {
            healthString = "0";
        } else if (currentHealth >= startHealth) {
            healthString = startHealth.ToString();
        }
        hpText.text = healthString;

        if (this.currentHealth > this.startHealth) {
            this.currentHealth = this.startHealth;
        }
        if (this.currentHealth < 0) {
            this.die();
        }
    }

    // Update is called once per frame
    void Update() {

        Text raccoonText = raccoonBar.GetComponentInChildren<Text>();
        string raccoonString = numRaccoons.ToString();
        if (numRaccoons < 0)
        {
            raccoonString = "0";
        }
        raccoonText.text = raccoonString;

        if (this.remainingIFrameTime > 0)
        {
            this.remainingIFrameTime -= Time.deltaTime;
        }

        if (timeSinceAttack < attackIntervalSeconds)
        {
            timeSinceAttack += Time.deltaTime;
        }

        Vector3 walkVector = Vector3.zero;

        if (state == PlayerStates.dead || state == PlayerStates.dying)
        {
            deadTime -= Time.deltaTime;
            if (deadTime <= 0) {
                Application.LoadLevel("GameOver");
            }
        }

        Camera cam = Camera.main;
        float camX = cam.transform.position.x;
        float maxL = camX - 3f;
        float maxR = camX + 3f;


        if (Input.GetKey(KeyCode.A) && transform.position.x >= maxL) {
            walkVector += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D) && transform.position.x <= maxR) {
            walkVector += Vector3.right;
        }
        if (Input.GetKey(KeyCode.W) && transform.position.y <= 0.75) {
            walkVector += (Vector3.up * ySpeedMult);
        }
        if (Input.GetKey(KeyCode.S) && transform.position.y >= -2) {
            walkVector += (Vector3.down * ySpeedMult);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            attack();
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            throwRaccoon();
        }

        if (walkVector != Vector3.zero) {
            this.walk(walkVector);
            animator.SetBool("walking", true);
        } else {
            animator.SetBool("walking", false);
        }

    }

    void attack() {
        if (timeSinceAttack < attackIntervalSeconds)
        {
            return;
        }
        timeSinceAttack = 0f;

        animator.SetTrigger("punch");
        Collider2D[] results = new Collider2D[20];

        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = enemiesMask;
        filter.useLayerMask = true;
        int resultCount = hurtbox.OverlapCollider(filter, results);
        Debug.Log("Attack contacting " + resultCount + " other things");

        for (int i = 0; i < resultCount; i++) {
            HipsterController enemy;
            enemy = results[i].GetComponentInParent<HipsterController>();
            enemy.Damage(punchDamage);
        }
    }

    void throwRaccoon() {
        if (this.numRaccoons <= 0)
        {
            return;
        }
        this.numRaccoons--;

        animator.SetTrigger("throwing");
        GameObject raccoonPrefab = Managers.GetInstance().GetGameProperties().RaccoonPrefab;

        GameObject raccoon = GameObject.Instantiate(raccoonPrefab);
        raccoon.transform.SetPositionAndRotation(transform.position, transform.rotation);


        Vector3 initialVelocity;

        if (transform.localScale.x < 0) {
            initialVelocity = new Vector3(-2, 2);
            Vector3 racScale = raccoon.transform.localScale;
            raccoon.transform.localScale = new Vector3(-1 * racScale.x, racScale.y, racScale.z);

        } else {
            initialVelocity = new Vector3(2, 2);
            
        }

        RaccoonController cntrl = raccoon.GetComponent<RaccoonController>();
        cntrl.SetVelocity(initialVelocity);
        cntrl.SetShadowY(transform.position.y - 0.25f);
    }

    void walk(Vector3 walkVector) {
        if (this.state == PlayerStates.dead || this.state == PlayerStates.dying) {
            return;
        }

        

        this.state = PlayerStates.walking;
        float step = speed * Time.deltaTime;

        transform.position += walkVector * step;
        if (walkVector.x > 0) {
            //transform.rotation = Quaternion.AngleAxis(180, Vector3.up);
            Vector3 newthing = this.initScale;
            //newthing.x *= -1;
            transform.localScale = newthing;
        }
        if (walkVector.x < 0) {
            //transform.rotation = Quaternion.identity;
            Vector3 newthing = this.initScale;
            newthing.x *= -1;
            transform.localScale = newthing;
        }

        if(transform.position.x > camera.transform.position.x && !Managers.GetInstance().GetStageManager().ActiveScene()) {
            camera.transform.Translate(step * Vector3.right);
        }
    }

}
