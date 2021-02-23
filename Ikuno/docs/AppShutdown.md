AppShutdown
===========

## 使用方法
最も簡単な方法は `IServiceCollection` の拡張メソッドに対して
ファイルパスを指定してサービスを登録することです。

```cs
// services is IServiceCollection instance
services.AddAppShutdownService(runningFilePath);
```

この方法では、サービス開始時に引数として渡したファイルパスに空のファイルを作成します。
そして、このファイルが削除、あるいは移動、名前の変更などがされたときにアプリケーションを終了します。

アプリケーションの終了条件を自分で定義したい場合は、
`ShutdownPredicate` デリゲートを渡すオーバーロードを使用します。
```cs
services.AddAppShutdownService(predicate);
```

このデリゲートは引数を取らず、bool型を戻り値とします。
この戻り値が `true` になったときにアプリケーションを終了します。

また、自作の `IFileOperator` インターフェイスを実装したクラスを用いることもできます。
```cs
class FileOperator : IFileOperator
{
	// ...
}
var fileOperator = new FileOperator();
services.AddSingleton<IFileOperator>(fileOperator);
services.AddHostedService<FileExistanceShutdownService>();
```
