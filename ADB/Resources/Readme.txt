.NET文档生成工具ADB已经为您创建了生成自定义文档生成器的解决方案{CustomBuilderName}

注意事项：
1.如果在生成工程后改变了ADB的安装位置，请重新配置工程的调试参数;
2.如果在生成工程后修改了{CustomBuilderName}的类名或其所在命名空间，请修改配置文件：
  Bin\Release\{CustomBuilderName}\{CustomBuilderName}.builder
  中<CustomBuilder>的Entry属性