using System;
using System.Collections.Generic;
using UnityEngine;

public class WordLibrary : MonoBehaviour
{
    public List<WordPair> wordPairs = new List<WordPair>();
    private System.Random random = new System.Random();

    // ��ӵ��ʶԵ����ʿ�
    public void AddWord(string english, string chinese)
    {
        WordPair pair = new WordPair();
        pair.englishWord = english;
        pair.chineseMeaning = chinese;
        wordPairs.Add(pair);
    }

    // �ӵ��ʿ��������ȡһ�����ʶ�
    public WordPair GetRandomWord()
    {
        if (wordPairs.Count == 0)
        {
            Debug.LogError("���ʿ�Ϊ�գ�����ӵ������ݡ�");
            return null;
        }
        int randomIndex = random.Next(0, wordPairs.Count);
        return wordPairs[randomIndex];
    }

    // ��ʼ�����ʿ�
    private void InitializeWordLibrary()
    {
        // ������
        AddWord("cat", "è");
        AddWord("dog", "��");
        AddWord("bird", "��");
        AddWord("fish", "��");
        AddWord("rabbit", "����");
        AddWord("monkey", "����");
        AddWord("elephant", "����");
        AddWord("tiger", "�ϻ�");
        AddWord("lion", "ʨ��");
        AddWord("panda", "��è");
        AddWord("bear", "��");
        AddWord("giraffe", "����¹");
        AddWord("zebra", "����");
        AddWord("kangaroo", "����");
        AddWord("fox", "����");
        AddWord("wolf", "��");
        AddWord("deer", "¹");
        AddWord("sheep", "����");
        AddWord("goat", "ɽ��");
        AddWord("cow", "��ţ");
        AddWord("horse", "��");
        AddWord("pig", "��");
        AddWord("duck", "Ѽ��");
        AddWord("chicken", "��");
        AddWord("hen", "ĸ��");
        AddWord("rooster", "����");
        AddWord("mouse", "����");
        AddWord("rat", "���󣨴�����");
        AddWord("hamster", "����");
        AddWord("snake", "��");
        AddWord("frog", "����");
        AddWord("turtle", "�ڹ�");
        AddWord("lizard", "����");
        AddWord("butterfly", "����");
        AddWord("bee", "�۷�");
        AddWord("ant", "����");
        AddWord("spider", "֩��");
        AddWord("worm", "���");
        AddWord("octopus", "����");
        AddWord("starfish", "����");
        AddWord("shark", "����");
        AddWord("dolphin", "����");
        AddWord("whale", "����");

        // ˮ����
        AddWord("apple", "ƻ��");
        AddWord("banana", "�㽶");
        AddWord("cherry", "ӣ��");
        AddWord("grape", "����");
        AddWord("lemon", "����");
        AddWord("lime", "���");
        AddWord("mango", "â��");
        AddWord("orange", "����");
        AddWord("peach", "����");
        AddWord("pear", "��");
        AddWord("pineapple", "����");
        AddWord("strawberry", "��ݮ");
        AddWord("watermelon", "����");
        AddWord("kiwi", "⨺���");
        AddWord("plum", "����");
        AddWord("apricot", "����");
        AddWord("blueberry", "��ݮ");
        AddWord("raspberry", "��ݮ");
        AddWord("blackberry", "��ݮ");
        AddWord("coconut", "Ҭ��");

        // ʳ����
        AddWord("bread", "���");
        AddWord("cake", "����");
        AddWord("cookie", "����");
        AddWord("pizza", "����");
        AddWord("hamburger", "������");
        AddWord("hot dog", "�ȹ�");
        AddWord("sandwich", "������");
        AddWord("noodle", "����");
        AddWord("rice", "�׷�");
        AddWord("soup", "��");
        AddWord("salad", "ɳ��");
        AddWord("egg", "����");
        AddWord("cheese", "����");
        AddWord("milk", "ţ��");
        AddWord("juice", "��֭");
        AddWord("water", "ˮ");
        AddWord("ice cream", "�����");
        AddWord("chocolate", "�ɿ���");
        AddWord("candy", "�ǹ�");
        AddWord("popcorn", "���׻�");

        // ��ɫ��
        AddWord("red", "��ɫ");
        AddWord("blue", "��ɫ");
        AddWord("yellow", "��ɫ");
        AddWord("green", "��ɫ");
        AddWord("orange", "��ɫ");
        AddWord("purple", "��ɫ");
        AddWord("pink", "��ɫ");
        AddWord("brown", "��ɫ");
        AddWord("black", "��ɫ");
        AddWord("white", "��ɫ");
        AddWord("gray", "��ɫ");

        // ������
        AddWord("one", "һ");
        AddWord("two", "��");
        AddWord("three", "��");
        AddWord("four", "��");
        AddWord("five", "��");
        AddWord("six", "��");
        AddWord("seven", "��");
        AddWord("eight", "��");
        AddWord("nine", "��");
        AddWord("ten", "ʮ");
        AddWord("eleven", "ʮһ");
        AddWord("twelve", "ʮ��");
        AddWord("thirteen", "ʮ��");
        AddWord("fourteen", "ʮ��");
        AddWord("fifteen", "ʮ��");
        AddWord("sixteen", "ʮ��");
        AddWord("seventeen", "ʮ��");
        AddWord("eighteen", "ʮ��");
        AddWord("nineteen", "ʮ��");
        AddWord("twenty", "��ʮ");
        AddWord("thirty", "��ʮ");
        AddWord("forty", "��ʮ");
        AddWord("fifty", "��ʮ");
        AddWord("sixty", "��ʮ");
        AddWord("seventy", "��ʮ");
        AddWord("eighty", "��ʮ");
        AddWord("ninety", "��ʮ");
        AddWord("hundred", "��");

        // ���岿λ��
        AddWord("head", "ͷ");
        AddWord("hair", "ͷ��");
        AddWord("eye", "�۾�");
        AddWord("ear", "����");
        AddWord("nose", "����");
        AddWord("mouth", "���");
        AddWord("tooth", "����");
        AddWord("tongue", "��ͷ");
        AddWord("neck", "����");
        AddWord("shoulder", "���");
        AddWord("arm", "�ֱ�");
        AddWord("elbow", "�ⲿ");
        AddWord("wrist", "����");
        AddWord("hand", "��");
        AddWord("finger", "��ָ");
        AddWord("chest", "�ز�");
        AddWord("stomach", "����");
        AddWord("back", "����");
        AddWord("waist", "����");
        AddWord("leg", "��");
        AddWord("knee", "ϥ��");
        AddWord("ankle", "����");
        AddWord("foot", "��");
        AddWord("toe", "��ֺ");

        // �ճ���Ʒ��
        AddWord("book", "��");
        AddWord("pencil", "Ǧ��");
        AddWord("pen", "�ֱ�");
        AddWord("ruler", "����");
        AddWord("eraser", "��Ƥ");
        AddWord("pencil case", "Ǧ�ʺ�");
        AddWord("schoolbag", "���");
        AddWord("desk", "����");
        AddWord("chair", "����");
        AddWord("bed", "��");
        AddWord("sofa", "ɳ��");
        AddWord("table", "����");
        AddWord("lamp", "̨��");
        AddWord("clock", "ʱ��");
        AddWord("key", "Կ��");
        AddWord("door", "��");
        AddWord("window", "����");
        AddWord("mirror", "����");
        AddWord("brush", "ˢ��");
        AddWord("toothbrush", "��ˢ");
        AddWord("toothpaste", "����");
        AddWord("soap", "����");
        AddWord("towel", "ë��");
        AddWord("clothes", "�·�");
        AddWord("shirt", "����");
        AddWord("pants", "����");
        AddWord("skirt", "ȹ��");
        AddWord("dress", "����ȹ");
        AddWord("shoe", "Ь��");
        AddWord("sock", "����");
        AddWord("hat", "ñ��");
        AddWord("gloves", "����");
        AddWord("umbrella", "��ɡ");

        // ��Ȼ��
        AddWord("sun", "̫��");
        AddWord("moon", "����");
        AddWord("star", "����");
        AddWord("cloud", "��");
        AddWord("rain", "��");
        AddWord("snow", "ѩ");
        AddWord("wind", "��");
        AddWord("tree", "��");
        AddWord("flower", "��");
        AddWord("grass", "��");
        AddWord("mountain", "ɽ");
        AddWord("river", "����");
        AddWord("lake", "����");
        AddWord("sea", "��");
        AddWord("beach", "��̲");
        AddWord("forest", "ɭ��");
        AddWord("field", "��Ұ");

        // ְҵ��
        AddWord("teacher", "��ʦ");
        AddWord("doctor", "ҽ��");
        AddWord("nurse", "��ʿ");
        AddWord("policeman", "����");
        AddWord("fireman", "����Ա");
        AddWord("postman", "�ʵ�Ա");
        AddWord("driver", "˾��");
        AddWord("farmer", "ũ��");
        AddWord("chef", "��ʦ");
        AddWord("actor", "��Ա");
        AddWord("actress", "Ů��Ա");
        AddWord("singer", "����");
        AddWord("dancer", "����");
        AddWord("artist", "������");
        AddWord("writer", "����");
        AddWord("scientist", "��ѧ��");
        AddWord("engineer", "����ʦ");

        // ��ͨ������
        AddWord("car", "����");
        AddWord("bus", "������");
        AddWord("train", "��");
        AddWord("bike", "���г�");
        AddWord("motorcycle", "Ħ�г�");
        AddWord("ship", "��");
        AddWord("plane", "�ɻ�");
        AddWord("subway", "����");
        AddWord("taxi", "���⳵");

        // ������
        AddWord("walk", "��·");
        AddWord("run", "�ܲ�");
        AddWord("jump", "��Ծ");
        AddWord("swim", "��Ӿ");
        AddWord("fly", "��");
        AddWord("sleep", "˯��");
        AddWord("eat", "��");
        AddWord("drink", "��");
        AddWord("read", "�Ķ�");
        AddWord("write", "��д");
        AddWord("draw", "����");
        AddWord("sing", "����");
        AddWord("dance", "����");
        AddWord("play", "��");
        AddWord("listen", "��");
        AddWord("watch", "�ۿ�");
        AddWord("talk", "��̸");
        AddWord("laugh", "Ц");
        AddWord("cry", "��");
        AddWord("think", "˼��");

        Debug.Log($"���ʿ��ʼ����ɣ���������: {wordPairs.Count}");
    }

    void Awake()
    {
        InitializeWordLibrary();
    }
}