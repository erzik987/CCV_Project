# CCV_Project

-Projekt sa dá ideálne spustiť cez Views/Acount/Login
-Musíte sa doň zaregistrovať, prvý zaregistrovaný sa stane automaticky adminom (berie sa to podla ID), 
ten má neskôr výhodu že po každom prihlásení môže spravovať všetkých prihlásených užívateľov
-Do projektu sa dá nahrať iba súbor .csv : v subore pre sklady je hlavička (prvý riadok sa nezapočíta do dát) a dalej názov,
číslo vyjadrujúce typ skladu(0=Dry , 1=Frish , 2=OG , default=Dry), a bool označujúci či je sklad aktívny (zadáva sa "true" alebo "false")
-Projekt nemá ošetrený import do databázy, takže sa dajú porušiť niektoré pravidlá ktoré sú inak nastavené(nemôžu byť dva storehousy s rovnakým názvom atď)
