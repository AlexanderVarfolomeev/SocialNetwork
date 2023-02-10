## Инструкция по запуску
<p>Рекомендуется запускать с помощью docker-compose.</p>
<p>В директории проекта:</p>

``docker-compose up --build``
___
## Описание проекта
<p>Социальная сеть - аналог VK/Facebook.</p>
<h3>Базовая функциональность:</h3>

- Возможность зарегистрировать аккаунт. 
- На личной странице пользователь может вести личный блог, выкладывать записи. 
- Есть возможность добавления пользователей в друзья. 
- Пользователь может создать в группу, в которой также есть возможность выкладывать записи. 
- Под записями можно оставлять комментарии. 
- Записи можно лайкать.
- Есть несколько ролей: 
  - Обычный пользователь
  - Администратор
  - Администратор с неограниченными правами
- Можно оставить жалобу на пользователя/комментарий/запись/группу, жалобы должен рассматривать администратор.
- Реализован встроенный мессенджер, есть возможность создать диалог/групповой чат.
- К постам/сообщениям/комментариям можно прикладывать картинки.
- Пользователь может поставить аватар.
- Возможность восстановить пароль.
- Возможность посмотреть глобальную ленту, с сортировкой по популярности (за день/неделю и тд).

<h3>Рассылка почты</h3>
- При регистрации приходит сообщение, в котором необходимо подтвердить регистрацию.
- При отправке жалобы приходит сообщение, об обработке жалобы.
- При бане/удалении аккаунта/поста и тд, приходит сообщение с предупреждением.
- При восстановлении пароля, письмо с подтверждением.

<h3>RabbitMQ</h3>
Для уменьшения нагрузки на систему, с помощью брокера очередей реализована следующая функциональность:
- Отправка жалоб.
- Рассылка о регистрации.
- Запросы на дружбу.
- Лайки.
- Различные уведомления: о лайках, новых комментариях, постах в группах и тд.

<h3>Redis</h3>
В качестве системы для кэширования используется Redis. Кэшируется следующая информация:
- Личная страница пользователя.
- Список друзей.
- Профили друзей.
- Список последних уведомлений.
- Список групп на которые подписан пользователь.
- Список последних постов в группах.
- Топ самых популярных записей.

<h3>SignalR</h3>
- Мессенджер.
- Уведомления. 
- Статус пользователь (онлай/офлайн)
- Добавление комментариев

:x: