using System;
using System.Collections.Generic;
using UnityEngine;

public class WordLibrary : MonoBehaviour
{
    public List<WordPair> wordPairs = new List<WordPair>();
    private System.Random random = new System.Random();

    // 添加单词对到单词库
    public void AddWord(string english, string chinese)
    {
        WordPair pair = new WordPair();
        pair.englishWord = english;
        pair.chineseMeaning = chinese;
        wordPairs.Add(pair);
    }

    // 从单词库中随机获取一个单词对
    public WordPair GetRandomWord()
    {
        if (wordPairs.Count == 0)
        {
            Debug.LogError("单词库为空，请添加单词数据。");
            return null;
        }
        int randomIndex = random.Next(0, wordPairs.Count);
        return wordPairs[randomIndex];
    }

    // 初始化单词库
    private void InitializeWordLibrary()
    {
        // 动物类
        AddWord("cat", "猫");
        AddWord("dog", "狗");
        AddWord("bird", "鸟");
        AddWord("fish", "鱼");
        AddWord("rabbit", "兔子");
        AddWord("monkey", "猴子");
        AddWord("elephant", "大象");
        AddWord("tiger", "老虎");
        AddWord("lion", "狮子");
        AddWord("panda", "熊猫");
        AddWord("bear", "熊");
        AddWord("giraffe", "长颈鹿");
        AddWord("zebra", "斑马");
        AddWord("kangaroo", "袋鼠");
        AddWord("fox", "狐狸");
        AddWord("wolf", "狼");
        AddWord("deer", "鹿");
        AddWord("sheep", "绵羊");
        AddWord("goat", "山羊");
        AddWord("cow", "奶牛");
        AddWord("horse", "马");
        AddWord("pig", "猪");
        AddWord("duck", "鸭子");
        AddWord("chicken", "鸡");
        AddWord("hen", "母鸡");
        AddWord("rooster", "公鸡");
        AddWord("mouse", "老鼠");
        AddWord("rat", "老鼠（大老鼠）");
        AddWord("hamster", "仓鼠");
        AddWord("snake", "蛇");
        AddWord("frog", "青蛙");
        AddWord("turtle", "乌龟");
        AddWord("lizard", "蜥蜴");
        AddWord("butterfly", "蝴蝶");
        AddWord("bee", "蜜蜂");
        AddWord("ant", "蚂蚁");
        AddWord("spider", "蜘蛛");
        AddWord("worm", "蠕虫");
        AddWord("octopus", "章鱼");
        AddWord("starfish", "海星");
        AddWord("shark", "鲨鱼");
        AddWord("dolphin", "海豚");
        AddWord("whale", "鲸鱼");

        // 水果类
        AddWord("apple", "苹果");
        AddWord("banana", "香蕉");
        AddWord("cherry", "樱桃");
        AddWord("grape", "葡萄");
        AddWord("lemon", "柠檬");
        AddWord("lime", "酸橙");
        AddWord("mango", "芒果");
        AddWord("orange", "橙子");
        AddWord("peach", "桃子");
        AddWord("pear", "梨");
        AddWord("pineapple", "菠萝");
        AddWord("strawberry", "草莓");
        AddWord("watermelon", "西瓜");
        AddWord("kiwi", "猕猴桃");
        AddWord("plum", "李子");
        AddWord("apricot", "杏子");
        AddWord("blueberry", "蓝莓");
        AddWord("raspberry", "树莓");
        AddWord("blackberry", "黑莓");
        AddWord("coconut", "椰子");

        // 食物类
        AddWord("bread", "面包");
        AddWord("cake", "蛋糕");
        AddWord("cookie", "饼干");
        AddWord("pizza", "披萨");
        AddWord("hamburger", "汉堡包");
        AddWord("hot dog", "热狗");
        AddWord("sandwich", "三明治");
        AddWord("noodle", "面条");
        AddWord("rice", "米饭");
        AddWord("soup", "汤");
        AddWord("salad", "沙拉");
        AddWord("egg", "鸡蛋");
        AddWord("cheese", "奶酪");
        AddWord("milk", "牛奶");
        AddWord("juice", "果汁");
        AddWord("water", "水");
        AddWord("ice cream", "冰淇淋");
        AddWord("chocolate", "巧克力");
        AddWord("candy", "糖果");
        AddWord("popcorn", "爆米花");

        // 颜色类
        AddWord("red", "红色");
        AddWord("blue", "蓝色");
        AddWord("yellow", "黄色");
        AddWord("green", "绿色");
        AddWord("orange", "橙色");
        AddWord("purple", "紫色");
        AddWord("pink", "粉色");
        AddWord("brown", "棕色");
        AddWord("black", "黑色");
        AddWord("white", "白色");
        AddWord("gray", "灰色");

        // 数字类
        AddWord("one", "一");
        AddWord("two", "二");
        AddWord("three", "三");
        AddWord("four", "四");
        AddWord("five", "五");
        AddWord("six", "六");
        AddWord("seven", "七");
        AddWord("eight", "八");
        AddWord("nine", "九");
        AddWord("ten", "十");
        AddWord("eleven", "十一");
        AddWord("twelve", "十二");
        AddWord("thirteen", "十三");
        AddWord("fourteen", "十四");
        AddWord("fifteen", "十五");
        AddWord("sixteen", "十六");
        AddWord("seventeen", "十七");
        AddWord("eighteen", "十八");
        AddWord("nineteen", "十九");
        AddWord("twenty", "二十");
        AddWord("thirty", "三十");
        AddWord("forty", "四十");
        AddWord("fifty", "五十");
        AddWord("sixty", "六十");
        AddWord("seventy", "七十");
        AddWord("eighty", "八十");
        AddWord("ninety", "九十");
        AddWord("hundred", "百");

        // 身体部位类
        AddWord("head", "头");
        AddWord("hair", "头发");
        AddWord("eye", "眼睛");
        AddWord("ear", "耳朵");
        AddWord("nose", "鼻子");
        AddWord("mouth", "嘴巴");
        AddWord("tooth", "牙齿");
        AddWord("tongue", "舌头");
        AddWord("neck", "脖子");
        AddWord("shoulder", "肩膀");
        AddWord("arm", "手臂");
        AddWord("elbow", "肘部");
        AddWord("wrist", "手腕");
        AddWord("hand", "手");
        AddWord("finger", "手指");
        AddWord("chest", "胸部");
        AddWord("stomach", "肚子");
        AddWord("back", "背部");
        AddWord("waist", "腰部");
        AddWord("leg", "腿");
        AddWord("knee", "膝盖");
        AddWord("ankle", "脚踝");
        AddWord("foot", "脚");
        AddWord("toe", "脚趾");

        // 日常用品类
        AddWord("book", "书");
        AddWord("pencil", "铅笔");
        AddWord("pen", "钢笔");
        AddWord("ruler", "尺子");
        AddWord("eraser", "橡皮");
        AddWord("pencil case", "铅笔盒");
        AddWord("schoolbag", "书包");
        AddWord("desk", "书桌");
        AddWord("chair", "椅子");
        AddWord("bed", "床");
        AddWord("sofa", "沙发");
        AddWord("table", "桌子");
        AddWord("lamp", "台灯");
        AddWord("clock", "时钟");
        AddWord("key", "钥匙");
        AddWord("door", "门");
        AddWord("window", "窗户");
        AddWord("mirror", "镜子");
        AddWord("brush", "刷子");
        AddWord("toothbrush", "牙刷");
        AddWord("toothpaste", "牙膏");
        AddWord("soap", "肥皂");
        AddWord("towel", "毛巾");
        AddWord("clothes", "衣服");
        AddWord("shirt", "衬衫");
        AddWord("pants", "裤子");
        AddWord("skirt", "裙子");
        AddWord("dress", "连衣裙");
        AddWord("shoe", "鞋子");
        AddWord("sock", "袜子");
        AddWord("hat", "帽子");
        AddWord("gloves", "手套");
        AddWord("umbrella", "雨伞");

        // 自然类
        AddWord("sun", "太阳");
        AddWord("moon", "月亮");
        AddWord("star", "星星");
        AddWord("cloud", "云");
        AddWord("rain", "雨");
        AddWord("snow", "雪");
        AddWord("wind", "风");
        AddWord("tree", "树");
        AddWord("flower", "花");
        AddWord("grass", "草");
        AddWord("mountain", "山");
        AddWord("river", "河流");
        AddWord("lake", "湖泊");
        AddWord("sea", "大海");
        AddWord("beach", "海滩");
        AddWord("forest", "森林");
        AddWord("field", "田野");

        // 职业类
        AddWord("teacher", "老师");
        AddWord("doctor", "医生");
        AddWord("nurse", "护士");
        AddWord("policeman", "警察");
        AddWord("fireman", "消防员");
        AddWord("postman", "邮递员");
        AddWord("driver", "司机");
        AddWord("farmer", "农民");
        AddWord("chef", "厨师");
        AddWord("actor", "演员");
        AddWord("actress", "女演员");
        AddWord("singer", "歌手");
        AddWord("dancer", "舞者");
        AddWord("artist", "艺术家");
        AddWord("writer", "作家");
        AddWord("scientist", "科学家");
        AddWord("engineer", "工程师");

        // 交通工具类
        AddWord("car", "汽车");
        AddWord("bus", "公交车");
        AddWord("train", "火车");
        AddWord("bike", "自行车");
        AddWord("motorcycle", "摩托车");
        AddWord("ship", "船");
        AddWord("plane", "飞机");
        AddWord("subway", "地铁");
        AddWord("taxi", "出租车");

        // 动作类
        AddWord("walk", "走路");
        AddWord("run", "跑步");
        AddWord("jump", "跳跃");
        AddWord("swim", "游泳");
        AddWord("fly", "飞");
        AddWord("sleep", "睡觉");
        AddWord("eat", "吃");
        AddWord("drink", "喝");
        AddWord("read", "阅读");
        AddWord("write", "书写");
        AddWord("draw", "画画");
        AddWord("sing", "唱歌");
        AddWord("dance", "跳舞");
        AddWord("play", "玩");
        AddWord("listen", "听");
        AddWord("watch", "观看");
        AddWord("talk", "交谈");
        AddWord("laugh", "笑");
        AddWord("cry", "哭");
        AddWord("think", "思考");

        Debug.Log($"单词库初始化完成，单词数量: {wordPairs.Count}");
    }

    void Awake()
    {
        InitializeWordLibrary();
    }
}