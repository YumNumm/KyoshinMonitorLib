# KyoshinMonitorLib
.NETから簡単に強震モニタを利用できるようにするライブラリ

主に個人用に開発したライブラリです。  
強震モニタを使用したソフトを開発する際に毎回クラスや処理をコピーするのが面倒なので作成しました。

# 更新情報
## 0.1.6
### 変更
- `KyoshinMonitorLib.Images` で色から震度に変換する際に7.0以上の色を追加しました。

## 0.1.5
### 変更
- `Nullable` パース問題のためJsonシリアライザを `Utf8Json` から `Newtonsoft.Json` に変更しました。

### 修正
- `WebApi.GetEewInfo` が正常に動作しなかった問題を解決しました。 thx! @Kichi2525yt

## 0.1.4.0
### 修正
- 観測地点のリンク処理を改良しました。  
  これにより今のところは割り当てられない観測地点はなくなったと思います。

## 0.1.3.1
### 修正
- 古い地点情報のデータを使用した際に正常に使用できなかった問題を修正しました。

## 0.1.3.0
### 変更
- `LinkedRealTimeData` に `Intensity` を追加しました。  
  `Value` がnullの場合でも正常に動作します。

## 0.1.2.0
### 変更
- `AppApi` の `GetHypoInfo` が分かりづらかったため、 `GetEewHypoInfo` に変更しました。
  - 従来のメソッドは非推奨になっています。

### 追加
- `AppApi` に以下のメソッドが追加されました。(詳細はリファレンスにて)
  - `GetPSWave`
  - `GetEstShindo`
  - `GetMeshes`

### バグ修正
- 0.1.1.0アプデート内容の誤字を修正しました。


## 0.1.1.1
### バグ修正
- `NtpAssistance` においてIPv6環境にてNTPでの時刻取得に失敗する問題を修正しました。

## 0.1.1.0
### バグ修正
- `GetLinkedRealTimeData`において休止中の観測点と結び付けないようにしました。
  - ついでに異常な値が入っててもそれなりに対処できるようにしました。

### 追加
- `GetHypoInfo`を追加しました。 詳細はリファレンス参照してください。
- `KyoshinMonitorLib.Training`を作成しました。
  - `TrainingAppApi`クラスを追加しました。  
    `AppApi`クラスを継承しており、ファイルからJsonなどを読み込むクラスとなっています。  
    - 解説を書く時間がなかったのでとりあえず使いたい人はソース読んでください…。

## 0.1.0.0
**全面的な内容の変更のため破壊的変更が多数含まれています。**
- **protobuf-netを切り捨て、MessagePackのみの対応になりました。**
- 従来の`拡張メソッドで地点情報の配列から情報を取得する`方針から、`APIを呼ぶためのインスタンスを作成し、そこから情報を取得する`方針へと変更になりました。
- **プロジェクトが分割されました。**
  - コアは`KyoshinMonitorLib`、画像を使用する場合は`KyoshinMonitorLib.Images`、タイマー関係は`KyoshinMonitorLib.Timers`になりました。
- `ObservationPoint`のプロパティ`OldLocation`(日本座標系)が追加されました。  
`GetLinkedRealTimeData`を使用する際にこれがセットされていないと正常にマッピングされません。
  - AppApiを使用して観測点情報をリンクさせる場合、旧座標のインポートが必要になります。

- **`SecondBasedTimer`の`Elapsed`イベントの返り値が`Task`になりました。**  
  これによりasync/awaitに対応しました。

*過去のアップデートは長くなるため省略しています。過去バージョンをご利用ください。*

# リファレンス
バージョン:`0.1.2.0`  
主要なクラスのみ解説します。詳細な解説はソースなどを参照してください。  
また、気象庁震度階級や地球の緯度経度など、前提知識が必要なものがあります。

## KyoshinMonitorExceptionクラス
強震モニタのAPIから情報を取得している間に、HTTPエラーまたはタイムアウトが発生した場合に発生する例外です。
### プロパティ
| 型 | 名前(引数) | 解説 |
|---|---|---|
|`string`|Message|どのような例外が発生したか|
|`Exception`|InnerException|内部で発生した例外|
|`string`|Url|例外が発生したAPIのURL  アクセス中でない場合はnullが代入されます。|
|`HttpStatusCode?`|StatusCode|HTTPエラーが発生した場合そのHTTPStatus  アクセス中でない場合はnullが代入されます。|

## ObservationPointクラス
[KyoshinShindoPlaceEditor](https://github.com/ingen084/KyoshinShindoPlaceEditor)と互換があります。
### LoadFromMpk/Json
```c#
public static ObservationPoint[] LoadFromMpk(string path, bool usingLz4 = false);
public static ObservationPoint[] LoadFromJson(string path);
```
観測点情報をmpk/jsonから読み込みます。失敗した場合は例外がスローされます。  
**lz4圧縮済みのmpkを通常のmpkとして読み込まないように注意してください。**

### LoadFromCsv
```c#
public static (ObservationPoint[] points, uint success, uint error) LoadFromCsv(string path, Encoding encoding = null);
```
観測点情報をcsvから読み込みます。失敗した場合は例外がスローされます。

### SaveToCsv/Mpk/Json
```c#
public static void SaveToCsv(string path, IEnumerable<ObservationPoint> points);
public static void SaveToMpk(string path, IEnumerable<ObservationPoint> points, bool useLz4 = false);
public static void SaveToJson(string path, IEnumerable<ObservationPoint> points);
```
拡張メソッド版
```c#
public static void SaveToCsv(this IEnumerable<ObservationPoint> points, string path);
public static void SaveToMpk(this IEnumerable<ObservationPoint> points, string path, bool useLz4 = false);
public static void SaveToJson(this IEnumerable<ObservationPoint> points, string path);
```
観測点情報を各形式に保存します。失敗した場合は例外がスローされます。


## WebApiクラス
Webで見ることができる強震モニタのAPIを使用してEEWなどの画像やデータを取得するためのクラスです。
### メソッド
| 返り値の型 | 名前(引数) | 解説 |
|---|---|---|
|`Task<Eew>`|GetEewInfo(`DateTime` time)|緊急地震速報のJsonを取得します。  EewクラスはJsonをそのままパースしたものです。 |
|`Task<byte[]>`|GetRealtimeImageData(`DateTime` time, `RealTimeDataType` dataType, `bool` isBehore = false)|リアルタイムな情報(リアルタイム･震度･加速度など)の画像のbyte配列を取得します。  画像解析まで行いたい場合は下記の拡張メソッドをご利用ください。|
|`Task<byte[]>`|GetEstShindoImageData(`DateTime` time)|緊急地震速報の予想震度の画像のbyte配列を取得します。|
|`Task<byte[]>`|GetPSWaveImageData(`DateTime` time)|緊急地震速報のP波とS波の広がりを示す円の画像のbyte配列を取得します。|

### KyoshinMonitorLib.Imagesによる拡張メソッド
| 返り値の型 | 名前(引数) | 解説 |
|---|---|---|
|`Task<IEnumerable<ImageAnalysisResult>>`|ParseIntensityFromParameterAsync(this `WebApi` webApi, `IEnumerable<ObservationPoint>` points, `DateTime` datetime, `bool` isBehole = false)|ObservationPointのコレクションを使用して新強震モニタリアルタイム震度の画像を取得し、解析します。|

他にもありますが割愛させていただきます。

#### 画像から震度を解析するにあたってのメモ

震度7以上観測された場合、必ず7.0が帰ってくるようです。  
`7.0+` のような表記を採用してあげるとユーザーに優しいかもしれません。  
尚、AppApiで取得した値は7.0以上でも正確に表示されます。

## AppApiクラス
スマートフォンアプリケーションのAPIを使用してリアルタイム震度などのデータを取得します。
### メソッド
| 返り値の型 | 名前(引数) | 解説 |
|---|---|---|
|`Task<LinkedRealTimeData[]>`|GetLinkedRealTimeData(`DateTime` time, `RealTimeDataType` dataType, `bool` isBehore = false)|リアルタイムデータを取得します。  自動で観測点情報などと結びつけ、インスタンスを返します。|
|`Task<RealTimeData>`|GetRealTimeData(`DateTime` time, `RealTimeDataType` dataType, `bool` isBehore = false)|リアルタイムデータを取得します。  特に理由がない限り`GetLinkedRealTimeData`を使用することを推奨します。|
|`Task<SiteList>`|GetSiteList(`string` baseSerialNo)|APIから参照できる観測点情報の一覧を取得します。  特に理由がない限り`GetLinkedRealTimeData`を使用することを推奨します。|
|`Task<Hypo>`|GetEewHypoInfo(`DateTime` time)|APIから緊急地震速報の情報を取得します。  **ちなみに、複数のEEWに対応してそうです…(要検証)**|
|`Task<PSWave>`|GetPSWave(`DateTime` time)|緊急地震速報から算出された揺れの広がりを取得します。  **こちらも、は複数のEEWに対応してそうです。**|
|`Task<EstShindo>`|GetEstShindo(`DateTime` time)|緊急地震速報から算出された予想震度の5kmメッシュ情報を取得します。|
|`Task<Mesh[]>`|GetMeshes()|メッシュ一覧を取得します。 非常に時間がかかるため、起動時などに行い、別ファイルとしてキャッシュしておくことを推奨します。|

### 重要事項
- `GetEewHypoInfo`
- `GetPSWave`
- `GetEstShindo`

**この3つのAPIはEEWが発表されていない場合は404が帰ってきます。**

## Meshクラス
5kmメッシュ情報を取り扱います。
### プロパティ
| 型 | 名前(引数) | 解説 |
|---|---|---|
|`string`|Code|メッシュのコード 詳細不明|
|`Location`|LocationLeftTop|右上(北西)の緯度経度|
|`Location`|LocationRightBottom|左下(南東)の緯度経度|

### 備考
使用方法の詳細は省略しますが、 `GetEstShindo` の返り値を見ればわかると思います。  
ですが需要があれば書くかもしれません。またお知らせください。


## UrlGeneratorクラス群
UrlGeneratorは分離した上に、各種Apiクラスでラップしているため、解説は省略させていただきます。

## FixedTimerクラス
通常のタイマー(System.Timers.Timerなど。FormsのTimerは申し訳ないが論外)では、誤差が蓄積していきずれていきますが、それを対策したものです。

### サンプル
```c#
//タイマーのインスタンスを作成(デフォルトは間隔1000ms+精度1ms↓)
var timer = new FixedTimer()
{
	Interval = TimeSpan.FromMilliseconds(500), //500msに設定
};
//適当にイベント設定
timer.Elapsed += () =>
{
	//適当な処理
};
//タイマー開始
timer.Start();
//改行入力待ち
Console.ReadLine();
//タイマーストップ
timer.Stop();
```

## SecondBasedTimerクラス
FixedTimerに時刻管理機能をつけたものです。  
強震モニタの取得タイマーとしてしか考慮していないので必ず1秒になります。

### 注意
時間の更新(補正)は自動でされないため、別途タイマーなどで実行してください。

### サンプル
```c#
//タイマーのインスタンスを作成(デフォルトは精度1ms↓)
var timer = new SecondBasedTimer()
{
	Offset = TimeSpan.FromSeconds(2.5), //イベントの発火時間を2500ms *後ろに* ずらす。だいたいこれ前後がおすすめ。
};
//適当にイベント設定
timer.Elapsed += time =>
{
	//timeに時間が入っているのでそれを使用して取得する
};
//タイマー開始 引数には現在の時刻が必要です。
await timer.Start(await NtpAssistance.GetNetworkTimeWithNtp());
//改行入力待ち
Console.ReadLine();
//タイマーストップ
timer.Stop();
```

## NtpAssistance
NTPから簡単に時刻取得をするクラスです。

### メソッド
| 返り値の型 | 名前(引数) | 解説 |
|---|---|---|
|`Task<DateTime?>`|GetNetworkTimeWithNtp(`string` hostName = "ntp.nict.jp", `ushort` port = 123, `int` timeout = 100)|SNTP通信を使用してネットワーク上から時刻を取得します。  一応SNTPを実装していますが、NICT以外のNTPサーバーでの挙動は保証しません。|
|`Task<DateTime?>`|GetNetworkTimeWithHttp(`string` url = "https://ntp-a1.nict.go.jp/cgi-bin/jst", `double` timeout = 1000)|Http通信を使用してネットワーク上から時刻を取得します。  小数のPOSIX Timeを含んだレスポンスが返されるURLであればなんでも使用できるとおもいます。|

## JmaIntensity
気象庁震度階級を示す列挙型(Enum)です。震度異常などを扱うために値が増やされています。

### サンプル
```c#
JmaIntensity shindo = 1.0f.ToJmaIntensity(); //JmaIntensity.Int1
Console.WriteLine(shindo.ToShortString()); //1
Console.WriteLine(shondo.ToLongString()); //震度1

Console.WriteLine("5+".ToJmaIntensity().ToLongString()); //文字からも解析できます。 出力:震度5強

float? invalidIntensity = null;
Console.WriteLine(invalidIntensity.ToJmaIntensity()); //nullableなfloatもできます。 出力:JmaIntensity.Unknown
```
