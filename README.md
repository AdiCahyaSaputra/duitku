# Jalanin di local komputer kalian

```bash
cp -r .env.example .env # Jangan lupa isi env nya (cuma JWT_KEY nya aja sih wkwkwk)

```

# Langsung build pake docker compose aja

```bash
docker-compose -f docker-compose.db.dev.yaml up --build -d && docker-compose -f docker-compose.app.dev.yaml up --build -d

```

# Command yang tadi menjalankan
- Build latest `pgsql`
- Build nuxt frontend
- Build asp.net api
- Update database via migration

![image](https://github.com/user-attachments/assets/73d78d8f-fd28-45d5-9dcc-043415882982)
- Windows Pride 🤘

# Info
- SDK .NET Core versi 8.0.402
- Bun versi 1.1.29
- ASP.NET Core 8 pake template webapi 
- NuxtJS + Shadcn/UI Vue (Typescript ready)

# Url
Pastiin `migration` nya udah kelar dulu ya bre sesuai yang di ss

- 🚀 Frontend : 
```bash
http://localhost:3000/login
```
- 🔥 API : 
```bash
http://localhost:8080/api
```
- 🤖 DB (konek-in pake dbeaver) : 
```bash
Host `localhost`
Port `5433`
Username `postgres`
Password `postgres`
Database `duit_ku`

```

[Postman Collection](https://github.com/user-attachments/files/17449701/duit-ku.postman_collection.json)
[Preview Demo Video](https://www.linkedin.com/posts/adi-cs_setelah-3-minggu-belajar-nuxtjs-vue-dan-activity-7254413256344182784-PaCY?utm_source=share&utm_medium=member_desktop)

# Todo
- [x] fix: Bug search items di combobox (kalo pake static data lancar" aja)
- [ ] feat: Prediksi hasil OCR pake OpenAI GPT buat deskripsi transaksi, tanggal, harga total instead of pake RegEx
