using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float m_StartingHealth = 100f;
    public Slider m_Slider;
    public Image m_FillImage;
    public Color m_FullHealthColor = Color.green;
    public Color m_ZeroHealthColor = Color.red;
    public GameObject m_ExplosionPrefab;

    private AudioSource m_ExplosionAudio;
    private ParticleSystem m_ExplosionParticles;

    private float m_CurrentHealth;
    private bool m_Dead;

    // Nuevas propiedades
    public bool m_HasShield { get; private set; } = false;
    private bool m_IsInvincible = false;

    private void Awake()
    {
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();
        m_ExplosionParticles.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;
        m_HasShield = false;
        m_IsInvincible = false;
        SetHealthUI();
    }

    public void TakeDamage(float amount)
    {
        if (m_IsInvincible)
            return;

        if (m_HasShield)
        {
            ToggleShield(0); // Desactiva el escudo al recibir daño
            return;
        }

        m_CurrentHealth -= amount;
        SetHealthUI();

        if (m_CurrentHealth <= 0f && !m_Dead)
        {
            OnDeath();
        }
    }

    private void SetHealthUI()
    {
        m_Slider.value = m_CurrentHealth;
        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
    }

    private void OnDeath()
    {
        m_Dead = true;
        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);
        m_ExplosionAudio.Play();
        m_ExplosionParticles.Play();
        gameObject.SetActive(false);
    }

    // ✅ Métodos nuevos para power-ups

    public void ToggleShield(float shieldAmount)
    {
        m_HasShield = !m_HasShield;
        // Puedes agregar efectos visuales aquí si deseas
    }

    public void ToggleInvincibility()
    {
        m_IsInvincible = !m_IsInvincible;
        // También puedes activar efectos visuales o sonidos
    }

    public void IncreaseHealth(float healAmount)
    {
        m_CurrentHealth = Mathf.Min(m_CurrentHealth + healAmount, m_StartingHealth);
        SetHealthUI();
    }
}
