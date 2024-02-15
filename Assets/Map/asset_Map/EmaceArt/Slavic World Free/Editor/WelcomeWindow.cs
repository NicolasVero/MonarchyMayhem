using UnityEditor;
using UnityEngine;

public class WelcomeWindow : EditorWindow
{
    private GUIStyle textureButton;
    private GUIStyle headingText;
    private GUIStyle commonText;

    private Texture2D top;

    private Texture2D image1;

    private Texture2D linkButton;

    private Texture2D logo;

    private Vector2 scrollIndex;

    [UnityEditor.Callbacks.DidReloadScripts]
    private static void OpenWindowOnUnityStart()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode)
            return;

        OpenWindow();
    }

    [MenuItem("Tools/EmaceArt/Welcome Window")]
    private static void OpenWindow()
    {
        EditorWindow panel = GetWindow<WelcomeWindow>();
        panel.titleContent = new GUIContent("Hello Developer!", Resources.Load<Texture2D>("Favi_top"));
        //  panel.minSize = new Vector2(600, Mathf.Min(Screen.currentResolution.height, 862));
        // panel.maxSize = new Vector2(600, Mathf.Min(Screen.currentResolution.height, 862));
        //panel.ShowUtility();
    }

    private void OnEnable()
    {
        var style = Resources.Load<GUISkin>("GUISkin");
        textureButton = style.GetStyle("textureButton");
        headingText = style.GetStyle("headingText");
        commonText = style.GetStyle("commonText");

        top = Resources.Load<Texture2D>("EA_Top");

        image1 = Resources.Load<Texture2D>("Btn_01");

        linkButton = Resources.Load<Texture2D>("button_free_zone");

        logo = Resources.Load<Texture2D>("Logo");
    }

    private void OnGUI()
    {
        scrollIndex = GUILayout.BeginScrollView(scrollIndex);
        GUILayout.BeginVertical();

        DrawHeader();
        GUILayout.Space(20f);
        DrawBody();
        GUILayout.Space(20f);
        DrawFooter();

        GUILayout.EndVertical();
        GUILayout.EndScrollView();
    }

    private void DrawHeader()
    {
        if (GUILayout.Button(top, textureButton))
            Application.OpenURL("https://assetstore.unity.com/packages/3d/environments/fantasy/slavic-medieval-village-town-interior-and-exterior-pack-environm-137794");
    }

    private void DrawBody()
    {
        GUILayout.Label("Thanks for checkin Slavica Lite", headingText);
        GUILayout.Space(1f);
        GUILayout.Label("This pack perfect match with the Medieval Slavica Town Full!", commonText);
        GUILayout.Space(20f);

        if (GUILayout.Button(image1, textureButton))
            Application.OpenURL("https://assetstore.unity.com/packages/3d/environments/fantasy/slavic-medieval-village-town-interior-and-exterior-pack-environm-137794");
    }

    private void DrawFooter()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(logo, textureButton))
            Application.OpenURL("https://www.emaceart.com");

        GUILayout.BeginVertical();
        GUILayout.Space(10f);
        GUILayout.Label("Visit my free zone. If you like this content, don't forget leave review :)", commonText);
        GUILayout.Space(10f);
        if (GUILayout.Button(linkButton, textureButton))
            Application.OpenURL("https://assetstore.unity.com/lists/free-zone-178789");

        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
    }
}