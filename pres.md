---
marp: false
theme: default
paginate: true
---

<style>
div.twocols {
  margin-top: 35px;
  column-count: 2;
}
div.twocols p:first-child,
div.twocols h1:first-child,
div.twocols h2:first-child,
div.twocols ul:first-child,
div.twocols ul li:first-child,
div.twocols ul li p:first-child {
  margin-top: 0 !important;
}
div.twocols p.break {
  break-before: column;
  margin-top: 0;
}
</style>

# Юнит тестирование

Шириев Рустем Венерович - тех. лид. команды разработки

Газпромнефть - Цифровые решения

---

###  Слайд в котором я рассказываю, что у нас круто работать 😎

#### Технологический режим 2.0

>комплексный инструмент оперативного управления фондом добывающих скважин, направленный на оптимизацию затрат на подъём скважинной продукции

>Программа ежедневного просмотра ВСЕХ скважин для анализа того, на сколько эффективнее можно качать нефть на них

<!-- стоит послушать всех докладчиков -->

---

### Пример информационной системы

#### Система бронирования коворкинг мест 📅

  - позволяет сотрудникам просматривать карту свободных рабочих мест (столов, переговорных, зон коворкинга)
  - резервировать их на нужный период через мобильное приложение
  - Система автоматически проверяет конфликты по времени, учитывает загруженность офиса
  - После бронирования пользователь получает подтверждение
  - Администратор может управлять конфигурацией офиса
  - Система интегрирована с корпоративной системой пользователей для получения данных о пользователе
  - Система интегрирована с аналитической системой офиса для подсчёта использования рабочих мест за период времени

<!-- У нас в офисе есть такая же -->

<style scoped>
h3, h4 {
  color: darkred;
}
</style>

---

### Пример информационной системы

#### Система бронирования коворкинг мест

```mermaid
flowchart TB
    user[🧑‍💼Пользователь]
    admin[🧑‍💻Администратор]
    account_system[📈Аналитическая система]
    user_base[📒База пользователей]
    br_system[📅Система бронирования коворкинг мест]:::current
    user --> br_system
    admin --> br_system
    account_system --> br_system
    br_system --> user_base
    
    classDef current fill:darkgreen, color:white
```

>Требуется внести правки в существующую систему

<!--
 - Что нужно сделать после разработки?
     - тут будет сказано что нужно тестировать
 - А вы что? Собираетесь ошибаться?
     - тут будет слайд почему люди ошибаются
-->

<style scoped>
h3, h4 {
  color: darkred;
}
</style>

---

### Ошибки при разработке ПО

Почему допускаются ошибки в ПО:

1. Программирование — это процесс познания
2. Невозможно держать в голове всю систему целиком
3. Окружение непредсказуемо
4. Когнетивные искажения в коммуникациях

~~Хороший программист пишет код так чтобы~~ Правильные процессы в команде разработки выстроены так, чтобы

 - Ошибки было легко найти
 - Ошибки были обнаружены как можно раньше
 - Ошибки не приводили к катастрофе

---

### Система бронирования коворкинг мест 📅

```mermaid
flowchart LR
    user[🧑‍💼Пользователь]
    admin[🧑‍💻Администратор]
    account_system[📈Аналитическая система]
    user_base[📒База пользователей]
    user --> ui
    admin --> ui
    account_system --> backend
    backend --> user_base
    subgraph br_system[📅 Система бронирования коворкинг мест]
        ui[Мобильное приложение]
        database[Сервер СУБД]
        backend[Сервер приложения]
        ui --> backend
        backend --> database
    end
```

<!--
 - А как мы можем протестировать?
     - мобильное приложение
     - нужно тестовое окружение
-->

<style scoped>
h3, h4 {
  color: darkred;
}
</style>

---

### Система бронирования коворкинг мест 📅

> Выделяем тестовое окружение

```mermaid
flowchart TB
    subgraph test_env[Тестовое окружение]
        direction LR
        qa[🧑‍🔬Тестировщик]
        account_system_test[📈Аналитическая система]
        user_base_test[📒База пользователей]
        qa --> ui_test
        qa -..- backend_test
        qa -..- database_test
        qa --> account_system_test
        qa --> user_base_test
        account_system_test --> backend_test
        backend_test --> user_base_test
        subgraph br_system_test[📅Система бронирования коворкинг мест]
            ui_test[Мобильное приложение]
            database_test[Сервер СУБД]
            backend_test[Сервер приложения]
            ui_test --> backend_test
            backend_test --> database_test
        end
    end
```

<!--
 - Выявление ошибок на этапе тестирование - дорогое удовольствие. Нужно это сдвигать
     - нужно автоматизировать тесты
-->

<style scoped>
h3, h4 {
  color: darkred;
}
</style>

---

### Автоматизированное тестирование ПО

> использует программные средства для выполнения тестов и проверки результатов выполнения, что помогает сократить время тестирования и упростить его процесс.
> 
> Википедия

---

### Пирамида тестирования

```mermaid
flowchart TD
    more_integration@{shape: brace-r, label: "Больше интеграции"} -..- more_isolation@{shape: brace-r, label: "Больше изоляции"}
    more_integration1@{shape: brace-r, label: "Медленнее"} -..- more_isolation1@{shape: brace-r, label: "Быстрее"}
    ui_tests@{ shape: tri, label: "UI тесты" }
    integration_tests@{ shape: trap-b, label: "интеграционные\nтесты" }
    unit_tests@{ shape: trap-b, label: "модульные тесты (unit тесты)" }
    ui_tests ~~~ integration_tests ~~~ unit_tests
    costly@{shape: brace-l, label: "Дороже"} -..- cheaper@{shape: brace-l, label: "Дешевле"}
```

> Майк Кон описал в книге «Scrum: гибкая разработка ПО»

---

## Цена ошибки

> Чем раньше обнаружена ошибка, тем дешевле её исправить

| Этап обнаружения ошибки   | Относительная стоимость исправления |
| ------------------------- | ----------------------------------- |
| Требования / Анализ       | 1x                                  |
| Проектирование            | 6–10x                               |
| Написание кода            | 15x                                 |
| Тестирование (QA)         | 40x                                 |
| После релиза (Production) | 100–1000x                           |

---

### Система бронирования коворкинг мест 📅

> добавляем авто тест

```mermaid
flowchart TB
    subgraph test_env[Тестовое окружение]
        direction LR
        qa[🤖Автотесты для Системы бронирования коворкинг мест]:::current
        account_system_test[📈Аналитическая система]
        user_base_test[📒База пользователей]
        qa e1@--> ui_test
        qa e2@--> account_system_test
        qa e3@--> user_base_test
        classDef current stroke-dasharray: 9,5,stroke-dashoffset: 900;
        class e1 current
        class e2 current
        class e3 current
        account_system_test --> backend_test
        backend_test --> user_base_test
        subgraph br_system_test[📅Система бронирования коворкинг мест]
            ui_test[Мобильное приложение]
            database_test[Сервер СУБД]
            backend_test[Сервер приложения]
            ui_test --> backend_test
            backend_test --> database_test
        end
    end
```

<!--
Автотесты интеграционные тоже очень дорогие
потому что
 - не все ситуации можно сымитировать
 - сложно выяснить где случилась поломка
 - тесты могут выполняться достаточно долго
 - тест достаточно сложно описать
-->

<style scoped>
h3, h4 {
  color: darkred;
}
</style>

---

### Система бронирования коворкинг мест 📅

> Модули

```mermaid
flowchart TB
    subgraph test_env[Тестовое окружение]
        direction LR
        account_system[📈Аналитическая система]
        user_base[📒База пользователей]
        account_system --> analytic_controller_module
        user_base_module --> user_base
        subgraph br_system[📅Система бронирования коворкинг мест]
            ui[Мобильное приложение]
            database[Сервер СУБД]
            backend[Сервер приложения]
            ui --> controller_module
            database_module --> database
            view_module --> ui
            subgraph backend[Сервер приложения]
                controller_module[Модуль обработки запроса на доступное место]
                analytic_controller_module[Модуль обработки запроса на аналитику]
                analytic_module[Модуль аналитики]
                logic_module[Модуль выполнения бронирования]
                calc_module[Модуль расчёта доступных мест]:::current
                database_module[Модуль работы с БД]
                view_module[Модуль отправки событий в мобильное приложени]
                user_base_module[Модуль работы с базой пользователей]
                controller_module --> logic_module
                logic_module --> calc_module
                logic_module --> database_module
                logic_module --> view_module
                logic_module --> user_base_module
                analytic_controller_module --> analytic_module
                analytic_module --> database_module
            end
        end
    end
    classDef current stroke:green
```

<style scoped>
h3, h4 {
  color: darkred;
}
</style>

---

### Система бронирования коворкинг мест 📅

> Божественный модуль

```mermaid
flowchart TB
    subgraph test_env[Тестовое окружение]
        direction LR
        account_system[📈Аналитическая система]
        user_base[📒База пользователей]
        account_system --> god_module
        god_module --> user_base
        subgraph br_system[📅Система бронирования коворкинг мест]
            ui[Мобильное приложение]
            database[Сервер СУБД]
            backend[Сервер приложения]
            ui --> god_module
            god_module --> database
            god_module --> ui
            subgraph backend[Сервер приложения]
                god_module[Божественный модуль]:::current
            end
        end
    end
    classDef current stroke:green
```

<style scoped>
h3, h4 {
  color: darkred;
}
</style>

---

### Unit-тест

>Unit-тест - код, написанный разработчиком, который проверяет небольшой кусок функциональности тестируемого кода.

Обратимся к википедии:  

> **Модульное тестирование**, или **юнит-тестирование** (англ. unit testing) — процесс в программировании, позволяющий проверить на корректность отдельные модули исходного кода программы.  
>   
> Идея состоит в том, чтобы писать тесты для каждой нетривиальной функции или метода. Это позволяет достаточно быстро проверить, не привело ли очередное изменение кода к регрессии, то есть к появлению ошибок в уже оттестированных местах программы, а также облегчает обнаружение и устранение таких ошибок.

---

### История появления и развития юнит тестов

```mermaid
timeline
	1956 : SAGE система наведения : Описаны первые принципы автоматизированных тестов : Х. Д. Бенингтон
	1964 : Проект Меркурий : Одни из первых реализиованных автотестов
    1989 : Первый фреймворк для юнит тестов SUnit для Smalltalk : Кент Бек
    1997 : Фреймворк для юнит тестов JUnit для Java : Кент Бек и Эрих Гамма 
	2005-2006 : Google интегрирует юнит тесты в свои разработки
```

> Википедия

---

### Сколько реально юнит тестов

```mermaid width=70% align=left
pie title JetBrains Developer Ecosystem 2025: Процент команд разработки
    "Покрывают автотестами > 50% кода" : 47
    "Покрывают автотестами < 50% кода" : 39
    "Не покрывают код автотестами" : 14
```

**Парадокс зрелости ([Covrig 2, ICST 2025](https://zenodo.org/records/14705473)):** в наиболее зрелых и надежных проектах наблюдается четкая тенденция: **количество строк кода, используемых для тестирования (TLOC), систематически превышает количество строк исполняемого кода (ELOC)**.

---

## Технический долг

> это метафора, описывающая последствия «быстрых» или «временных» решений в коде, которые в будущем требуют доработок (рефакторинга) (Уорд Каннингем)

<div class="twocols">

### Аналогия с банком
 - Взяли кредит: Сделали фичу быстро (грязно).
 - Платим проценты: Каждое изменение дается тяжелее, баги множатся.
 - Закрываем долг: Переписываем модуль (рефакторинг).

```mermaid
xychart-beta
    title "Sales Revenue"
    x-axis "Время" %% [jan, feb, mar, apr, may, jun, jul, aug, sep, oct]
    y-axis "Объём функционала" 0 --> 100
    line [0, 42, 50, 58, 60, 62]
    line [0, 20, 38, 56, 75, 90]
```

</div>

---

### Система бронирования коворкинг мест 📅

> Модульный тест

```mermaid
flowchart TB
    subgraph backend[Код Сервер приложения]
        controller_module[Модуль обработки запроса на доступное место]
        analytic_controller_module[Модуль обработки запроса на аналитику]
        analytic_module[Модуль аналитики]
        logic_module[Модуль выполнения бронирования]
        calc_module[Модуль расчёта доступных мест]:::current
        database_module[Модуль работы с БД]
        view_module[Модуль отправки событий в мобильное приложени]
        user_base_module[Модуль работы с базой пользователей]
        controller_module --> logic_module
        logic_module --> calc_module
        logic_module --> database_module
        logic_module --> view_module
        logic_module --> user_base_module
        analytic_controller_module --> analytic_module
        analytic_module --> database_module
    end
    
    subgraph calc_module_test_env[Код Сервер приложения]
        calc_module_test[Юнит тесты для модуля расчёта доступных мест] --> calc_module_tested[Модуль расчёта доступных мест]
    end


    classDef current stroke:green
```

<style scoped>
h3, h4 {
  color: darkred;
}
</style>

<!-- А как нам тестировать модули с зависимостями -->

---

TODO Добавить слайд про моки

---

### Система бронирования коворкинг мест 📅

> Модульный тест с моками

```mermaid
flowchart TB
    subgraph backend[Код Сервер приложения]
        controller_module[Модуль обработки запроса на доступное место]
        analytic_controller_module[Модуль обработки запроса на аналитику]
        analytic_module[Модуль аналитики]
        logic_module[Модуль выполнения бронирования]
        calc_module[Модуль расчёта доступных мест]:::current
        database_module[Модуль работы с БД]
        view_module[Модуль отправки событий в мобильное приложени]
        user_base_module[Модуль работы с базой пользователей]
        controller_module --> logic_module
        logic_module --> calc_module
        logic_module --> database_module
        logic_module --> view_module
        logic_module --> user_base_module
        analytic_controller_module --> analytic_module
        analytic_module --> database_module
    end
    
    subgraph logic_module_test_env[Код Сервер приложения]
        logic_module_test[Юнит тесты модуля выполнения бронирования] --> logic_module_tested[Модуль выполнения бронирования]
        logic_module_tested --> calc_module_mock
        logic_module_tested --> database_module_mock
        logic_module_tested --> view_module_mock
        logic_module_tested --> user_base_module_mock
        logic_module_test -..-> calc_module_mock
        logic_module_test -..-> database_module_mock
        logic_module_test -..-> view_module_mock
        logic_module_test -..-> user_base_module_mock
    end

    classDef current stroke:green
```

<style scoped>
h3, h4 {
  color: darkred;
}
</style>

---

### Система бронирования коворкинг мест 📅

<div class="twocols">

> Модульный тест

```mermaid
flowchart TB
    subgraph calc_module_test_env[Код Сервер приложения]
        calc_module_tested[Модуль расчёта доступных мест]
        calc_module_test --> calc_module_tested
    end

    classDef current stroke:green
```

Описание метода:
- Проверка, можно ли занять место

<p class="break"></p>

<small>

 - Вход:
   - запрашиваемый интервал (начало, конец) - **requested**
   - список уже занятых интервалов - **bookedIntervals**
   - минимальный промежуток между посещением - **minGapMinutes**
 - Выход: Возвращает true, если интервал свободен, не пересекается с занятыми, и до/после него есть зазор не меньше заданного

</small>

</div>

<style scoped>
h3, h4 {
  color: darkred;
}
</style>

---

### Система бронирования коворкинг мест 📅

<div class="twocols">

```mermaid width=80%
flowchart TD
    Start([Начало]) --> GetFirst[Взять первый занятый интервал]
    
    ReturnTrue --> End([Конец])
    
    GetFirst --> Expand[Расширить интервал на minGapMinutes в обе стороны]
    
    Expand --> CheckGap{{запрашиваемый интервал пересекается с расширенным?}}
    
    CheckGap -->|Да| ReturnFalse[Вернуть false]
    ReturnFalse --> End
    
    CheckGap -->|Нет| NextInterval{{Есть ещё интервалы?}}
    
    NextInterval -->|Да| GetNext[Взять следующий занятый интервал]
    GetNext --> Expand
    
    NextInterval -->|Нет| ReturnTrue
    
    style Start fill:#90EE90
    style End fill:#FFB6C1
    style ReturnFalse fill:#FFB347
    style ReturnTrue fill:#87CEEB
    style CheckGap fill:#FFD700
    style NextInterval fill:#FFD700
```

> Модульный тест

```mermaid width=60%
flowchart TB
    Input[Входные данные: \n\n requested: 10:00-11:00 \n bookedIntervals: 09:00-09:30 \n minGapMinutes: 30 мин]

    Input --> Process[Вызвать метод 'Проверка, можно ли занять место']

    Process --> Check{{Метод вернул значение true}}
    
    Check -->|Да| Success[Тест пройден]
    Check -->|Нет| Fail[Тест провален]
    
    style Success fill:#FFB347
    style Fail fill:red
    style Process fill:#87CEEB
```

</div>

<style scoped>
h3, h4 {
  color: darkred;
}
</style>

<!--
ДЕМОНСТРАЦИЯ
-->

---

### Свойства хорошего unit теста (FIRST)

 - F — Fast (Быстрые)
 - I — Independent / Isolated (Независимые / Изолированные)
 - R — Repeatable (Повторяемые)
 - S — Self‑Validating (Самопроверяемые)
 - T — Thorough (Тщательные)

> Tim Ottinger and Brett Schuchert

---

### Свойства хорошего unit теста (FIRST)

#### F — Fast (Быстрые)

unit‑тесты должны выполняться очень быстро (миллисекунды или секунды). Это важно, потому что:

 - в проекте может быть тысячи unit‑тестов;
 - разработчики запускают их часто (при каждом изменении кода);
 - медленные unit‑тесты замедляют разработку и снижают мотивацию их запускать.
---

### Свойства хорошего unit теста (FIRST)

#### I — Independent / Isolated (Независимые / Изолированные)

Каждый unit‑тест должен:

 - выполняться независимо от других;
 - не зависеть от порядка запуска;
 - настраивать собственное окружение и очищать его после выполнения;
 - не использовать глобальное или разделяемое состояние.

---

### Свойства хорошего unit теста (FIRST)

#### R — Repeatable (Повторяемые)

unit‑тесты должны давать одинаковый результат при каждом запуске в одной и той же среде. На результат не должны влиять:

 - внешние факторы (сеть, база данных, файловая система);
 - дата/время;
 - случайные значения (если они не зафиксированы);
 - параллельное выполнение других тестов.

---

### Свойства хорошего unit теста (FIRST)

#### S — Self‑Validating (Самопроверяемые)

Тест должен сам определять, пройден он или нет. Для этого:

 - используются утверждения (assert, assertEquals и т. д.);
 - результат — чётко «прошёл» или «не прошёл»;
 - не требуется ручная проверка логов или вывода;
 - нет необходимости в дополнительной интерпретации результата.

---

### Свойства хорошего unit теста (FIRST)

#### T — Thorough (Тщательные)

unit‑тесты должны покрывать:
 - основные сценарии использования;
 - граничные случаи (крайние значения, ошибки, исключения);
 - важные аспекты поведения кода (не только 100 % покрытие строк).

---

### Свойства хорошего unit теста (FIRST)

 - Если тест медленный → попробуйте убрать зависимости от БД/сети, использовать моки.

 - Если тесты зависят друг от друга → убедитесь, что каждый настраивает своё окружение.

 - Если тест иногда падает без причины → устраните внешние зависимости и случайные данные.

 - Если результат не очевиден → добавьте чёткие assert.

 - Если все тесты проходят, но часто выявляются ошибки в модулях → то следует тщательнее подойти к набору сценариев

---

### AAA (Arrange, Act, Assert) паттерн

Если посмотреть на юнит тест, то для большинства можно четко выделить 3 части кода:

 - **Arrange (настройка)** — в этом блоке кода мы настраиваем тестовое окружение тестируемого юнита;
 - **Act** — выполнение или вызов тестируемого сценария;
 - **Assert** — проверка того, что тестируемый вызов ведет себя определенным образом.

<!-- Демонстрация -->

---

### Итого

 - Ошибки в разработке ПО неизбежны и требуется их выявлять как можно раньше
 - Выявлять ошибки на ранних этапах разработки помогает юнит тестирование
 - Юнит тесты (модульные тесты) - код, который проверяет основной код программы на корректную реализацию логики
 - Отсутствие юнит тестов в проекте - признак наличия тех долга
 - Юнит тесты должны выполняться быстро, изолированно, повторяемо, самопроверяемо, своевременно (FIRST: Fast, Isolated, Repeatable, Self-Validating, Timely)
 - Юнит тесты должны явно выделать блоки подготовки данных и объектов (Arrange), выполнения тестируемого метода (Act), проверки результата (Assert)

---

### Спасибо за внимание

Что почитать на тему:
 - Рой Ошероув - Искусство юнит-тестирования с примерами на JavaScript/С# (The Art of Unit Testing)
 - Владимир Хориков - Принципы юнит-тестирования
 - Кент Бек - Экстремальное программирование. Разработка через тестирование
