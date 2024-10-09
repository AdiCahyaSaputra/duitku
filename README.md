# Jalanin di local komputer kalian

```bash
cp -r .env.example .env # Jangan lupa isi env nya (cuma JWT_KEY nya aja sih wkwkwk)

```

# Langsung build pake docker compose aja

```bash
docker-compose -f docker-compose.db.dev.yaml up --build && docker-compose -f docker-compose.app.dev.yaml up --build

```

# Command yang tadi menjalankan
- Build latest `pgsql`
- Build nuxt frontend
- Build asp.net api
- Update database via migration

![image](https://github.com/user-attachments/assets/73d78d8f-fd28-45d5-9dcc-043415882982)

# Info
- SDK .NET Core versi 8.0.402
- Bun versi 1.1.29
- ASP.NET Core 8 pake template webapi 
- NuxtJS + Shadcn/UI Vue (Typescript ready)
