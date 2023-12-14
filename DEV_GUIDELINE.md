### Technical debits

- [ ] Naming refactoring according to the naming convention
- [ ] Create model conversion layer: nameModel < - > nameEntity
- [ ] Refactor to use Tasks properly

### Naming

| Item                    |  Notation  |
|:------------------------|:----------:|
| Namespace               | PascalCase |
| Class/Record            | PascalCase |
| Constructor             | PascalCase |
| Method Name             | PascalCase |
| Method Parameter        | camelCase  |
| Local variable          | camelCase  |
| static readonly         | PascalCase |
| public Class Attribute  | PascalCase |
| private Class Attribute | _camelCase |

## Configs

### SMTP

host: smtp.gmail.com
port: 25
user: carlossilvameloo@gmail.com
pass: cieorpmzfeoqzwfh


## Lint
1. [On Rider/intellij] Configure the code analyser: https://jetbrains.com/help/rider/StyleCop_Styles.html#applying-settings-from-settings-stylecop-files
2. [On Rider/intellij] Configure the code formatter: https://www.jetbrains.com/help/rider/Code_Formatting_Style.html


## References

1. DOCS: https://blazor.radzen.com/

2. course: https://www.youtube.com/watch?v=q4dV5qTqz6Q&list=PLmx8ERLT7ojMBv889LGUlcxlq3XQUltkc

3. icons: https://jossef.github.io/material-design-icons-iconfont/

4. Object Builder doc: https://www.nuget.org/packages/Object.Builder

5. HTMl to PDF: https://www.milanjovanovic.tech/blog/how-to-easily-create-pdf-documents-in-aspnetcore

6. storybook: https://github.com/jsakamoto/BlazingStory

https://github.com/jincod/dotnetcore-buildpack


heroku login
heroku container:login
heroku container:push web -a geniawebapp
heroku container:release web -a geniawebapp
heroku open --app geniawebapp
heroku logs --tail -a geniawebapp