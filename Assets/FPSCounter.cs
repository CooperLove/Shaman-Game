
using UnityEngine;
using System.Collections;
 
public class FPSCounter : MonoBehaviour {
	public Rect startRect = new Rect( 10, 30, 50, 50 ); // The rect the window is initially displayed at.
	public bool updateColor = true; // Do you want the color to change if the FPS gets low
	public bool allowDrag = true; // Do you want to allow the dragging of the FPS window
	public  float frequency = 0.5F; // The update frequency of the fps
	public int nbDecimal = 1; // How many decimal do you want to display
 
	private float accum   = 0f; // FPS accumulated over the interval
	private int   frames  = 0; // Frames drawn over the interval
	private Color color = Color.white; // The color of the GUI, depending of the FPS ( R < 10, Y < 30, G >= 30 )
	private string sFPS = ""; // The fps formatted into a string.
	private GUIStyle style; // The style the text will be displayed at, based en defaultSkin.label.

	public enum FpsCounterAnchorPositions { TopLeft, BottomLeft, TopRight, BottomRight };
	public FpsCounterAnchorPositions AnchorPosition = FpsCounterAnchorPositions.TopLeft;
	private FpsCounterAnchorPositions last_AnchorPosition;
	Vector2 currentPosition;

	Camera m_camera;
	public bool showFPS;

    private static FPSCounter instance;

    public static FPSCounter Instance { get => instance; set => instance = value; }

    FPSCounter (){
		if (Instance == null)
			Instance = this;
	}
 
	void Start()
	{
		m_camera = Camera.main;
	    StartCoroutine( FPS() );
		showFPS = false;
	}
 
	void Update()
	{
	    accum += Time.timeScale/ Time.deltaTime;
	    ++frames;
	}
 
	IEnumerator FPS()
	{
		// Infinite loop executed every "frenquency" secondes.
		while( true )
		{
			// Update the FPS
		    float fps = accum/frames;
		    sFPS = fps.ToString( "f" + Mathf.Clamp( nbDecimal, 0, 10 ) );
 
			//Update the color
			color = (fps >= 30) ? Color.green : ((fps > 10) ? Color.red : Color.yellow);
 
	        accum = 0.0F;
	        frames = 0;
 
			yield return new WaitForSeconds( frequency );
		}
	}
 
	void OnGUI()
	{
		
		if (!showFPS)
			return;
		// Copy the default label skin, change the color and the alignement
		if( style == null ){
			style = new GUIStyle( GUI.skin.label );
			style.normal.textColor = Color.white;
			style.alignment = TextAnchor.MiddleCenter;
		}
 
		GUI.color = updateColor ? color : Color.white;
		if (AnchorPosition != last_AnchorPosition){
            currentPosition = Set_FrameCounter_Position(AnchorPosition);
			Debug.Log(m_camera.ViewportToScreenPoint(new Vector3(1, 1, 0.0f)));
			//startRect.x = currentPosition.x;
			//startRect.y = currentPosition.y;
			last_AnchorPosition = AnchorPosition;
		}
		startRect = GUI.Window(0, startRect, DoMyWindow, "");
	}
 
	void DoMyWindow(int windowID)
	{
		GUI.Label( new Rect(0, 0, startRect.width, startRect.height), sFPS + " FPS", style );
		if( allowDrag ) GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
	}

	Vector3 Set_FrameCounter_Position(FpsCounterAnchorPositions anchor_position)
    {
		switch (anchor_position)
		{
			case FpsCounterAnchorPositions.TopLeft:
				return m_camera.ViewportToScreenPoint(new Vector3(0, 1, 0.0f));
			case FpsCounterAnchorPositions.BottomLeft:
				return m_camera.ViewportToScreenPoint(new Vector3(0, 0, 0.0f));
			case FpsCounterAnchorPositions.TopRight:
				return m_camera.ViewportToScreenPoint(new Vector3(1, 1, 0.0f));
			case FpsCounterAnchorPositions.BottomRight:
				return m_camera.ViewportToScreenPoint(new Vector3(1, 0, 0.0f));
		}
		return new Vector3();
    }
}