using UnityEngine;

public class ScalePlayer : MonoBehaviour
{
    public AudioClip aMinorClip; // Drag your A minor scale audio clip here
    public float timeBetweenNotes = 0.5f;

    private AudioSource audioSource;

    float transpose = -4;  // transpose in semitones
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayAMinorScale();
        }
        
        var note = -1; // invalid value to detect when note is pressed
        if (Input.GetKeyDown("a")) note = 0;  // C
        if (Input.GetKeyDown("s")) note = 2;  // D
        if (Input.GetKeyDown("d")) note = 4;  // E
        if (Input.GetKeyDown("f")) note = 5;  // F
        if (Input.GetKeyDown("g")) note = 7;  // G
        if (Input.GetKeyDown("h")) note = 9;  // A
        if (Input.GetKeyDown("j")) note = 11; // B
        if (Input.GetKeyDown("k")) note = 12; // C
        if (Input.GetKeyDown("l")) note = 14; // D
	
        if (note>=0){ // if some key pressed...
            audioSource.pitch =  Mathf.Pow(2, (note + transpose)/12.0f);
            audioSource.Play();
        }
    }

    private void PlayAMinorScale()
    {
        // A minor scale notes in MIDI note values
        int[] aMinorScale = { 57, 59, 60, 62, 64, 65, 67, 69, 71, 72 };

        foreach (int note in aMinorScale)
        {
            PlayNote(note);
            // Wait for the specified time before playing the next note
            System.Threading.Thread.Sleep((int)(timeBetweenNotes * 1000));
        }
    }

    private void PlayNote(int midiNote)
    {
        // Convert MIDI note value to frequency
        float frequency = Mathf.Pow(2f, (midiNote - 69) / 12.0f) * 440.0f;

        // Set the audio clip to the A minor scale clip
        audioSource.clip = aMinorClip;

        // Modify pitch to match the frequency of the note
        audioSource.pitch = frequency / audioSource.clip.length;

        // Play the audio clip
        audioSource.Play();

        // Wait for the clip to finish playing before moving to the next note
        System.Threading.Thread.Sleep((int)(audioSource.clip.length * 1000));
    }
}