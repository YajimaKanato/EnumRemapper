# Enum Remapper

Unity の ScriptableObject に保存された enum を、定義変更後にメンバー名で引き直す Editor ツールです。要素の途中挿入や明示値の変更で数値が変わっても、同じメンバーを選択した状態を維持します。

## UPM でインストール

Unity 6.0 以降と Git が必要です。Package Manager の **Install package from git URL** に次を入力します。

```text
https://github.com/YajimaKanato/EnumRemapper.git?path=/EnumRemapper
```

配布タグを固定する場合は、公開済みのタグを URL の末尾に付けます。たとえば `v1.0.0` を公開した後は、導入先の `Packages/manifest.json` の `dependencies` に次の項目を追加できます。

```json
"com.knty1018.enum-remapper": "https://github.com/YajimaKanato/EnumRemapper.git?path=/EnumRemapper#v1.0.0"
```

ローカルの変更を試す場合は **Install package from disk** で、このリポジトリの `EnumRemapper/package.json` を選びます。

## 使い方

```csharp
using EnumRemapper.Runtime;

[EnumRemap]
public enum CharacterState
{
    Idle = 0,
    Running = 10
}
```

独自の `.asmdef` 内で属性を使う場合は、そのアセンブリから `EnumRemapper.Runtime` を参照します。

1. enum に属性を付け、値を変更する前にコンパイルします。`ProjectSettings/EnumCache.json` に変更前の定義が記録されます。
2. ScriptableObject の Inspector で `Running` を選び、アセットを保存します。
3. `Running = 20` に変えてコンパイルします。Inspector は `Running` のままで、アセットの保存値は `20` に更新されます。

更新はスクリプトのリロード後に自動実行されます。手動実行は **Tools > Enum Remap > Update** です。チームで作業する場合は、enum 定義、変更されたアセット、`ProjectSettings/EnumCache.json` を同じ変更に含めます。

## 対応範囲

- `Assets` 配下の主アセットである ScriptableObject の、Inspector に表示されるシリアライズ済みフィールドが対象です。通常のフィールド、配列、リスト、入れ子のシリアライズ型に対応します。
- 基底型が `int` の enum が対象です。Prefab、Scene、サブアセット、`[Flags]` の組み合わせ値は対応範囲外です。
- 旧値に複数の名前が対応する場合は、そのフィールドをスキップして警告します。選択していたメンバーが削除または改名された場合も、保存値を維持します。
- 初回登録や旧形式キャッシュの切り替えでは、現在の定義を基準として記録します。すでに変更した定義から変更前の選択を復元することはできません。
