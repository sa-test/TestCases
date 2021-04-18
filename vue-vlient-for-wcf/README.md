# vue-vlient-for-wcf 

Это frontend клиент, к backend части написанной в предыдущем примере TestWCFServiceApp

Это мой первый опыт работы с frontendб так что пример заработал "не благодаря, а вопреки".
Уверен, в примере многое (или даже все?) можно переделать более правильно с методологической точки зрения.
Возможно я буду обновлять этот клиент с получением новых знаний.

Используется:
- vue, vuex, axios

Проблемы которые я не смог решить (был бы рад помощи от эксперта):
- PUT, DELETE has been blocked by CORS policy. 
Клиент и сервер запущены на локалхосте на разных портах.
Так и не смог разобраться с настройками сервера, чтобы PUT, DELETE могли работать.
Чтобы закончить пример использовал для всех операций GET.
Знаю, это очень плохо.
- не смог разобраться с v-model для vuex.
- на WCF сервисе пришлось создать отдельные функции для веб фронтенда. Требовались чтобы все параметры были string, 
а старые функции запрашивали int id и они используются в WinForm клиенте. Чтобы не переделывать WinForm клиент, сделал новую функцию.

## Project setup
```
npm install
```

### Compiles and hot-reloads for development
```
npm run serve
```

### Compiles and minifies for production
```
npm run build
```

### Lints and fixes files
```
npm run lint
```

### Customize configuration
See [Configuration Reference](https://cli.vuejs.org/config/).
