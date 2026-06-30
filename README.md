# EnumRemappingの使い方
### インストール方法
Window/Package Management/Package Manager/Install package from git URL に  
https://github.com/YajimaKanato/EnumRemapper.git?path=/EnumRemapper  
を張り付け

## 使い方
enumの定義をするときに[Enum]をつけるだけです

## EnumRemappingでできること
enumに要素を追加した際に、Inspectorで変更前に選択していた要素を保持することができる
### 具体例
以下のようにenumを定義します
``` C#
public enum Test
{
	Idle,
	Move,
	Attack,
	Jump
}
```

Inspectorからこのenumをフィールドに定義しているところで "Idle" を設定します

以下のようにenumに要素を追加します
``` C#
public enum Test
{
	Die,		// 追加
	Idle,
	Move,
	Attack,
	Jump
}
```
本来であればInspectorを見た時に、Idleに設定していたところがDieに変わっていますが、 __Idleの設定状態を維持することができます__ 

## 注意点
現在はプロトタイプレベルの実装になるため、ScriptableObjectにしか対応できません
