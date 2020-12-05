﻿using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

class Tool_UIEditor : EditorWindow
{
    int _Options_Type;
    int _Options_Style;
    GameObject _CreatedCanvas;

    [MenuItem("Tools/Tool_UIEditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(Tool_UIEditor));
    }

    void OnGUI()
    {
        _Options_Type = GUILayout.Toolbar(_Options_Type, new string[] { "All" ,"Menu", "HUD" });

        if(_Options_Type == 0)
        {
            _Options_Style = GUILayout.Toolbar(_Options_Style, new string[] { "Default" });//, "CSGO", "Overwatch", "Minecraft", "RocketLeague" });
        }
        else
        {
            _Options_Style = GUILayout.Toolbar(_Options_Style, new string[] { "Default" });//, "CSGO", "Overwatch", "Minecraft", "RocketLeague" });
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Create"))
            CreateUI();
        if (GUILayout.Button("Delete"))
            if(_CreatedCanvas != null)
            {
                DestroyImmediate(_CreatedCanvas);
                _CreatedCanvas = null;

            }
        GUILayout.EndHorizontal();

        GUILayout.Label("Info:");
        _CreatedCanvas = (GameObject)EditorGUILayout.ObjectField("Created object", _CreatedCanvas, typeof(GameObject), true);

    }

    void CreateUI()
    {
        switch (_Options_Style)
        {
            case 0:
                CreateUI_Default();
                break;
        }
    }

    void CreateUI_Default()
    {
        //Add canvas
        GameObject canvasobj = CreateCanvas();

        //Add setting tabs
        GameObject tab_display = Create_Tab(canvasobj, "Display");
        GameObject tab_graphics = Create_Tab(canvasobj, "Graphics");
        GameObject tab_gameplay = Create_Tab(canvasobj, "Gameplay");
        GameObject tab_controls = Create_Tab(canvasobj, "Controls");
        GameObject tab_keybinding = Create_Tab(canvasobj, "Keybinding");

        //Add Buttons
        GameObject main = new GameObject();
        RectTransform mainrect = main.AddComponent<RectTransform>();
        SetRect(mainrect, "bottomleft");
        GameObject button_start = Create_Button(main, "Button_Start", "Start", new Vector2(40,450), new Vector2(700,100), 30, 60, "bottomleft");
        GameObject button_options = Create_Button(main, "Button_Options", "Options", new Vector2(40, 330), new Vector2(700, 100), 30, 60, "bottomleft");
        GameObject button_quit = Create_Button(main, "Button_Quit", "Quit", new Vector2(40, 210), new Vector2(700, 100), 30, 60, "bottomleft");
        main.name = "Main";
        main.transform.SetParent(canvasobj.transform);

        _CreatedCanvas = canvasobj;
    }

    GameObject CreateCanvas()
    {
        GameObject canvasobj = new GameObject();
        canvasobj.name = "TestCanvas";
        canvasobj.AddComponent<Canvas>();
        CanvasScaler canvasscale = canvasobj.AddComponent<CanvasScaler>();
        canvasscale.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasscale.referenceResolution = new Vector2(1920, 1080);
        canvasobj.AddComponent<GraphicRaycaster>();

        if (GameObject.Find("EventSystem") == null)
        {
            GameObject eventsystemobj = new GameObject();
            eventsystemobj.name = "EventSytem";
            eventsystemobj.AddComponent<EventSystem>();
            eventsystemobj.AddComponent<StandaloneInputModule>();
        }

        Canvas canvascomponent = canvasobj.GetComponent<Canvas>();
        canvascomponent.renderMode = RenderMode.ScreenSpaceCamera;
        
        return canvasobj;
    }
    GameObject Create_Button(GameObject parentobj ,string name, string buttontext, Vector2 pos, Vector2 size, float textoffset, float textsize, string anchorpos)
    {
        GameObject buttontemplate = new GameObject();
        RectTransform buttontransform = buttontemplate.AddComponent<RectTransform>();

        buttontransform.sizeDelta = size;
        buttontransform.anchoredPosition = pos;

        SetRect(buttontransform, anchorpos);

        buttontemplate.AddComponent<CanvasRenderer>();
        Image buttonimage = buttontemplate.AddComponent<Image>();
        buttonimage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        buttonimage.type = Image.Type.Sliced;
        Button buttonbutton = buttontemplate.AddComponent<Button>();
        buttonbutton.targetGraphic = buttonimage;
        buttontemplate.name = name;

        GameObject buttontextemplate = new GameObject();
        RectTransform buttontextrect = buttontextemplate.AddComponent<RectTransform>();

        buttontextrect.anchoredPosition = new Vector2(pos.x + textoffset, pos.y);
        buttontextrect.sizeDelta = size;
        buttontextrect.pivot = new Vector2(0,0);

        TextMeshProUGUI buttontexttmpro = buttontextemplate.AddComponent<TextMeshProUGUI>();
        buttontexttmpro.text = buttontext;
        buttontexttmpro.fontSize = textsize;
        buttontexttmpro.alignment = TextAlignmentOptions.MidlineLeft;
        buttontexttmpro.color = Color.black;

        buttontextemplate.name = name + "text";



        buttontextemplate.transform.SetParent(buttontemplate.transform);

        buttontemplate.transform.SetParent(parentobj.transform);

        return buttontemplate;
    }
    GameObject Create_Dropdown(GameObject parentobj, string name, string buttontext, Vector2 pos, Vector2 size, float textoffset, float textsize, string anchorpos)
    {
        //Create objects
        GameObject dropdownobj = new GameObject();
        GameObject dropdown_label = Create_Text(dropdownobj, "Text_DropDown", pos, size, "Option A", textsize, Color.black);
        GameObject dropdown_arrow = new GameObject();
        GameObject dropdown_template = new GameObject();

        GameObject dropdown_viewport = new GameObject();
        GameObject dropdown_content = new GameObject();
        GameObject dropdown_item = new GameObject();

        GameObject dropdown_scrollbar = new GameObject();
        GameObject dropdown_slidingarea = new GameObject();
        GameObject dropdown_handle = new GameObject();

        dropdown_template.SetActive(false);

        //Set Name
        dropdownobj.name = name;
        dropdown_label.name = "Label";
        dropdown_arrow.name = "Arrow";
        dropdown_template.name = "Template";

        dropdown_viewport.name = "Viewport";
        dropdown_content.name = "Conten";
        dropdown_item.name = "Item" +
            "";

        dropdown_scrollbar.name = "Scrollbar";
        dropdown_slidingarea.name = "Sliding Area";
        dropdown_handle.name = "Handle";

        //Add RectTransform
        RectTransform dropdownobjrect = dropdownobj.AddComponent<RectTransform>();
        RectTransform dropdown_arrowrect = dropdown_arrow.AddComponent<RectTransform>();
        RectTransform dropdown_templaterect = dropdown_template.AddComponent<RectTransform>();

        RectTransform dropdown_viewportrect = dropdown_viewport.AddComponent<RectTransform>();
        RectTransform dropdown_contentrect = dropdown_content.AddComponent<RectTransform>();
        RectTransform dropdown_itemrect = dropdown_item.AddComponent<RectTransform>();

        RectTransform dropdown_scrollbarrect = dropdown_scrollbar.AddComponent<RectTransform>();
        RectTransform dropdown_slidingarearect = dropdown_slidingarea.AddComponent<RectTransform>();
        RectTransform dropdown_handlerect = dropdown_handle.AddComponent<RectTransform>();

        //SetParent
        dropdown_label.transform.SetParent(dropdownobj.transform);
        dropdown_arrow.transform.SetParent(dropdownobj.transform);
        dropdown_template.transform.SetParent(dropdownobj.transform);

        dropdown_viewport.transform.SetParent(dropdown_template.transform);
        dropdown_content.transform.SetParent(dropdown_viewport.transform);
        dropdown_item.transform.SetParent(dropdown_content.transform);

        dropdown_scrollbar.transform.SetParent(dropdown_template.transform);
        dropdown_slidingarea.transform.SetParent(dropdown_scrollbar.transform);
        dropdown_handle.transform.SetParent(dropdown_slidingarea.transform);

        //Set Rect dropdownobj
        dropdownobjrect.sizeDelta = size;
        dropdownobjrect.anchoredPosition = pos;
        Image dropdownimage = dropdownobj.AddComponent<Image>();
        dropdownimage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        dropdownimage.type = Image.Type.Sliced;
        TMP_Dropdown dropdowntmp = dropdownobj.AddComponent<TMP_Dropdown>();
        SetRect(dropdownobjrect, anchorpos);

        //Set Rect Arrow
        dropdown_arrowrect.anchorMin = new Vector2(1, 0.5f);
        dropdown_arrowrect.anchorMax = new Vector2(1, 0.5f);
        dropdown_arrowrect.pivot = new Vector2(0.5f, 0.5f);
        dropdown_arrowrect.sizeDelta = new Vector2(20, 20);
        dropdown_arrowrect.anchoredPosition = new Vector4(-15, 0);

        //Set Rect Template
        dropdown_templaterect.anchorMin = new Vector2(0, 0);
        dropdown_templaterect.anchorMax = new Vector2(1, 0);
        dropdown_templaterect.pivot = new Vector2(0.5f, 1);
        dropdown_templaterect.sizeDelta = new Vector2(0, 150);
        dropdown_templaterect.anchoredPosition = new Vector4(0, 2);

        //Set Rect Viewport
        dropdown_viewportrect.anchorMin = new Vector2(0, 0);
        dropdown_viewportrect.anchorMax = new Vector2(1, 1);
        dropdown_viewportrect.pivot = new Vector2(0, 1);
        dropdown_viewportrect.sizeDelta = new Vector2(18, 0);
        dropdown_viewportrect.anchoredPosition = new Vector4(0, 0);

        //Set Rect Content
        dropdown_contentrect.anchorMin = new Vector2(0, 1);
        dropdown_contentrect.anchorMax = new Vector2(1, 1);
        dropdown_contentrect.pivot = new Vector2(0.5f, 1);
        dropdown_contentrect.sizeDelta = new Vector2(0, 28);
        dropdown_contentrect.anchoredPosition = new Vector4(0, 0);

        //Set Rect Item
        dropdown_itemrect.anchorMin = new Vector2(0, 0.5f);
        dropdown_itemrect.anchorMax = new Vector2(1, 0.5f);
        dropdown_itemrect.pivot = new Vector2(0.5f, 0.5f);
        dropdown_itemrect.sizeDelta = new Vector2(0, 20);
        dropdown_itemrect.anchoredPosition = new Vector4(0, 0);

        //Set Rect Scrollbar
        dropdown_scrollbarrect.anchorMin = new Vector2(1, 0);
        dropdown_scrollbarrect.anchorMax = new Vector2(1, 1);
        dropdown_scrollbarrect.pivot = new Vector2(1, 1);
        dropdown_scrollbarrect.sizeDelta = new Vector2(20, 0);
        dropdown_scrollbarrect.anchoredPosition = new Vector4(0, 0);

        //Set Rect Sliding Area
        dropdown_slidingarearect.anchorMin = new Vector2(0, 0);
        dropdown_slidingarearect.anchorMax = new Vector2(1, 1);
        dropdown_slidingarearect.pivot = new Vector2(0.5f, 0.5f);
        dropdown_slidingarearect.sizeDelta = new Vector2(10, 10);
        dropdown_slidingarearect.anchoredPosition = new Vector4(10, 10);

        //Set Rect Handle
        dropdown_handlerect.anchorMin = new Vector2(0, 0);
        dropdown_handlerect.anchorMax = new Vector2(1, 0.2f);
        dropdown_handlerect.pivot = new Vector2(0.5f, 0.5f);
        dropdown_handlerect.sizeDelta = new Vector2(-10, -10);
        dropdown_handlerect.anchoredPosition = new Vector4(-10, -10);


        dropdownobj.transform.SetParent(parentobj.transform);

        return dropdownobj;
    }
    GameObject Create_Text(GameObject parentobj, string name, Vector2 pos, Vector2 size, string textcontent, float fontsize, Color textcolor)
    {
        GameObject newtextobj = new GameObject();
        RectTransform buttontextrect = newtextobj.AddComponent<RectTransform>();
        newtextobj.name = name;
        buttontextrect.sizeDelta = size;
        buttontextrect.anchoredPosition = pos;
        SetRect(buttontextrect, "bottomleft");

        TextMeshProUGUI newtext = newtextobj.AddComponent<TextMeshProUGUI>();
        newtext.text = textcontent;
        newtext.fontSize = fontsize;
        newtext.alignment = TextAlignmentOptions.MidlineLeft;
        newtext.color = textcolor;

        newtext.transform.SetParent(parentobj.transform);
        return newtextobj;
    }
    GameObject Create_Slider(GameObject parentobj, string name, string buttontext, Vector2 pos, Vector2 size, float textoffset, float textsize, string anchorpos)
    {
        //Create Objects
        GameObject newsliderbackground = new GameObject();
        GameObject newsliderobj = new GameObject();
        GameObject newsliderfillarea = new GameObject();
        GameObject newsliderfill = new GameObject();
        GameObject newsliderslidearea = new GameObject();
        GameObject newsliderhandle = new GameObject();

        newsliderobj.name = name;

        //Set Parents
        newsliderbackground.transform.SetParent(newsliderobj.transform);
        newsliderfill.transform.SetParent(newsliderfillarea.transform);
        newsliderfillarea.transform.SetParent(newsliderobj.transform);
        newsliderhandle.transform.SetParent(newsliderslidearea.transform);
        newsliderslidearea.transform.SetParent(newsliderobj.transform);
        
        //Add RectTransform
        RectTransform buttonobjrect = newsliderobj.AddComponent<RectTransform>();
        RectTransform newsliderbackgroundrect = newsliderbackground.AddComponent<RectTransform>();
        RectTransform buttonfillarearect = newsliderfillarea.AddComponent<RectTransform>();
        RectTransform buttonfillrect = newsliderfill.AddComponent<RectTransform>();
        RectTransform buttonslidearearect = newsliderslidearea.AddComponent<RectTransform>();
        RectTransform buttonhandlerect = newsliderhandle.AddComponent<RectTransform>();

        //Add Images
        Image newsliderbackgroundimage = newsliderbackground.AddComponent<Image>();
        Image newsliderfillimage = newsliderfill.AddComponent<Image>();
        newsliderfillimage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        newsliderfillimage.type = Image.Type.Sliced;
        newsliderfillimage.color = Color.grey;
        Image newsliderhandleimage = newsliderhandle.AddComponent<Image>();

        //Set Rect NewObj
        buttonobjrect.sizeDelta = size;
        buttonobjrect.anchoredPosition = pos;
        SetRect(buttonobjrect, anchorpos);
        Slider newsliderslider = newsliderobj.AddComponent<Slider>();
        
        //Set Rect Background
        newsliderbackgroundrect.anchorMin = new Vector2(0, 0.25f);
        newsliderbackgroundrect.anchorMax = new Vector2(1, 0.75f);
        newsliderbackgroundrect.pivot = new Vector2(0.5f,0.5f);
        newsliderbackgroundrect.sizeDelta = new Vector2(0,0);
        newsliderbackgroundrect.anchoredPosition = new Vector2(0, 0);
        newsliderbackground.name = "BackGround";

        //Set Rect FillArea
        buttonfillarearect.anchorMin = new Vector2(0, 0.25f);
        buttonfillarearect.anchorMax = new Vector2(1, 0.75f);
        buttonfillarearect.pivot = new Vector2(0.5f, 0.5f);
        buttonfillarearect.sizeDelta = new Vector2(15, 0);
        buttonfillarearect.anchoredPosition = new Vector2(5, 0);
        newsliderfillarea.name = "FillArea";

        //Set Rect Fill
        buttonfillrect.anchorMin = new Vector2(0, 0.25f);
        buttonfillrect.anchorMax = new Vector2(1, 0.75f);
        buttonfillrect.pivot = new Vector2(0.5f, 0.5f);
        buttonfillrect.sizeDelta = new Vector2(10, 0);
        buttonfillrect.anchoredPosition = new Vector4(0,0);
        newsliderfill.name = "Fill";

        //Set Rect SliderArea
        buttonslidearearect.anchorMin = new Vector2(0, 0);
        buttonslidearearect.anchorMax = new Vector2(1, 1);
        buttonslidearearect.pivot = new Vector2(0.5f, 0.5f);
        buttonslidearearect.sizeDelta = new Vector2(10, 0);
        buttonslidearearect.anchoredPosition = new Vector2(10, 0);
        newsliderslidearea.name = "Handle Slide Area";
         
        //Set Rect Handle
        buttonhandlerect.anchorMin = new Vector2(0, 0.25f);
        buttonhandlerect.anchorMax = new Vector2(1, 0.75f);
        buttonhandlerect.pivot = new Vector2(0.5f, 0.5f);
        buttonhandlerect.sizeDelta = new Vector2(20, 0);
        buttonhandlerect.anchoredPosition = new Vector2(0, 0);
        newsliderhandleimage.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/Knob.psd");
        newsliderslider.image = newsliderhandleimage;
        newsliderslider.fillRect = buttonfillrect;
        newsliderslider.handleRect = buttonhandlerect;
        newsliderhandle.name = "Handle";

        newsliderobj.transform.SetParent(parentobj.transform);

        return newsliderobj;
    }
    GameObject Create_Tab(GameObject parentobj, string name)
    {
        GameObject tab_new = new GameObject();
        RectTransform tab_newrect = tab_new.AddComponent<RectTransform>();
        SetRect(tab_newrect, "bottomleft");
        tab_new.name = name;

        GameObject textobj = Create_Text(tab_new, "Title_" + name, new Vector2(800,800), new Vector2(1000,200), name, 100, Color.white);

        switch (name)
        {
            case "Display":
                GameObject button_Resolution = Create_Dropdown(tab_new, "Dropdown_Resolution", "Resolution", new Vector2(800, 700), new Vector2(500, 60), 10, 40, "bottomleft");
                GameObject Button_Fullscreen = Create_Button(tab_new, "Button_Fullscreen", "Fullscreen", new Vector2(800, 630), new Vector2(500, 60), 10, 40, "bottomleft");
                GameObject Button_Quality = Create_Button(tab_new, "Button_Quality", "Quality", new Vector2(800, 560), new Vector2(500, 60), 10, 40, "bottomleft");
                GameObject Button_VSync = Create_Button(tab_new, "Button_VSync", "VSync", new Vector2(800, 490), new Vector2(500, 60), 10, 40, "bottomleft");
                GameObject Button_MaxFPS = Create_Button(tab_new, "Button_MaxFPS", "MaxFPS", new Vector2(800, 420), new Vector2(500, 60), 10, 40, "bottomleft");
                GameObject Button_Gamma = Create_Button(tab_new, "Button_Gamma", "Gamma", new Vector2(800, 350), new Vector2(500, 60), 10, 40, "bottomleft");
                // Resolution
                // Fullscreen
                // Quality
                // V-Sync
                // Max fps
                // Gamma
                break;
            case "Graphics":
                GameObject button_Antialiasing = Create_Button(tab_new, "Button_Antialiasing", "Antialiasing", new Vector2(800, 700), new Vector2(500, 60), 10, 40, "bottomleft");
                GameObject Button_Shadows = Create_Button(tab_new, "Button_Shadows", "Shadows", new Vector2(800, 630), new Vector2(500, 60), 10, 40, "bottomleft");
                GameObject Button_ViewDistance = Create_Button(tab_new, "Button_ViewDistance", "ViewDistance", new Vector2(800, 560), new Vector2(500, 60), 10, 40, "bottomleft");
                GameObject Button_TextureQuality = Create_Button(tab_new, "Button_TextureQuality", "TextureQuality", new Vector2(800, 490), new Vector2(500, 60), 10, 40, "bottomleft");
                GameObject Button_ViolageDistance = Create_Button(tab_new, "Button_ViolageDistance", "ViolageDistance", new Vector2(800, 420), new Vector2(500, 60), 10, 40, "bottomleft");
                GameObject Button_ViolageDensity = Create_Button(tab_new, "Button_ViolageDensity", "ViolageDensity", new Vector2(800, 350), new Vector2(500, 60), 10, 40, "bottomleft");
                // Antialiasing
                // Shadows
                // ViewDistance
                // TextureQuality
                // ViolageDistance
                // ViolageDensity
                break;
            case "Gameplay":
                GameObject button_SoundEffects = Create_Button(tab_new, "Button_SoundEffects", "SoundEffects", new Vector2(800, 700), new Vector2(500, 60), 10, 40, "bottomleft");
                GameObject Button_Music = Create_Slider(tab_new, "Slider_Music", "Music", new Vector2(800, 630), new Vector2(500, 60), 10, 40, "bottomleft");
                // SoundEffects
                // Music
                break;
            case "Controls":

                break;
            case "Keybinding":

                break;
        }


        tab_new.transform.SetParent(parentobj.transform);
        tab_new.SetActive(false);
        return tab_new;
    }
    
    void SetRect(RectTransform rect, string anchorpos)
    {
        switch (anchorpos)
        {
            case "topleft":
                rect.anchorMin = new Vector2(0, 1);
                rect.anchorMax = new Vector2(0, 1);
                rect.pivot = new Vector2(0, 1);
                break;
            case "topmiddle":
                rect.anchorMin = new Vector2(0.5f, 1);
                rect.anchorMax = new Vector2(0.5f, 1);
                rect.pivot = new Vector2(0.5f, 1);
                break;
            case "topright":
                rect.anchorMin = new Vector2(1, 1);
                rect.anchorMax = new Vector2(1, 1);
                rect.pivot = new Vector2(1, 1);
                break;
            case "rightmiddle":
                rect.anchorMin = new Vector2(1, 0.5f);
                rect.anchorMax = new Vector2(1, 0.5f);
                rect.pivot = new Vector2(1, 0.5f);
                break;
            case "bottomright":
                rect.anchorMin = new Vector2(1, 0);
                rect.anchorMax = new Vector2(1, 0);
                rect.pivot = new Vector2(1, 0);
                break;
            case "bottommiddle":
                rect.anchorMin = new Vector2(0.5f, 0);
                rect.anchorMax = new Vector2(0.5f, 0);
                rect.pivot = new Vector2(0.5f, 0);
                break;
            case "bottomleft":
                rect.anchorMin = new Vector2(0, 0);
                rect.anchorMax = new Vector2(0, 0);
                rect.pivot = new Vector2(0, 0);
                break;
            case "leftmiddle":
                rect.anchorMin = new Vector2(0, 0.5f);
                rect.anchorMax = new Vector2(0, 0.5f);
                rect.pivot = new Vector2(0, 0.5f);
                break;
            case "middle":
                rect.anchorMin = new Vector2(0.5f, 0.5f);
                rect.anchorMax = new Vector2(0.5f, 0.5f);
                rect.pivot = new Vector2(0.5f, 0.5f);
                break;
        }
    }
}