using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBoard : MonoBehaviour
{
    private Queue<Message> mMessages;

    [SerializeField]
    private GameObject mMessagePrefab;

    [SerializeField]
    private int DisplayTimeInSeconds = 5;

    private static MessageBoard sInstance;

    // Start is called before the first frame update
    void Start()
    {
        mMessages = new Queue<Message>();
        sInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SendMessage(string text, Color color, FontStyle style = FontStyle.Normal)
    {
        var message = new Message(text, color, style);
        sInstance.StartCoroutine(sInstance.DisplayMessage(message));
    }

    private IEnumerator DisplayMessage(Message message)
    {
        var o =Instantiate(mMessagePrefab);
        var textComp = o.GetComponent<Text>();
        textComp.text = message.MessageText;
        textComp.color = message.Color;
        textComp.fontStyle = message.FontEffects;

        o.transform.SetParent(transform);

        yield return new WaitForSeconds(DisplayTimeInSeconds);

        while (textComp.color.a > 0f)
        {
            textComp.color = new Color(textComp.color.r, textComp.color.g, textComp.color.b, textComp.color.a - Time.deltaTime);
            yield return null;
        }

        Destroy(o);
    }

    struct Message
    {
        private Color mColor;
        public Color Color => mColor;
        private string mMessageText;
        public string MessageText => mMessageText;

        private FontStyle mFontStyle;
        public FontStyle FontEffects => mFontStyle;


        public Message(string text, Color color, FontStyle style)
        {
            mMessageText = text;
            mColor = color;
            mFontStyle = style;
        }
    }
}
