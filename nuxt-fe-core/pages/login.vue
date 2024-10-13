<script setup lang="ts">
import { toTypedSchema } from "@vee-validate/zod";
import * as z from "zod";
import { useForm } from "vee-validate";
import { useToast } from "@/components/ui/toast/use-toast";
import { useUser } from "@/composables/use-user";
import { login } from "@/services/auth.service";
import type LoginUserDto from "@/dto/LoginUserDto";
import type { ApiErrorDto } from "@/dto/BaseResponseDto";

definePageMeta({
  layout: "default",
});

useHead({
  title: "Login",
});

const inputItems: { name: string; placeholder: string; type: string }[] = [
  {
    name: "email",
    placeholder: "Email",
    type: "email",
  },
  {
    name: "password",
    placeholder: "Password",
    type: "password",
  },
];

const formSchema = toTypedSchema(
  z.object({
    email: z
      .string({ message: "Email harus di isi" })
      .email("Email nya nggak valid bre")
      .min(1, "Email harus di isi"),
    password: z
      .string({ message: "Password harus di isi" })
      .min(8, "Password harus diatas atau 8 karakter"),
  }),
);

const form = useForm({
  validationSchema: formSchema,
});

const { toast } = useToast();
const { setAuthToken } = useUser();

const {
  isPending,
  isError,
  mutate: loginMutate,
} = useMutation({
  mutationFn: login,
  onSuccess: (res: any) => {
    toast({
      title: "Notifikasi",
      description: "Teleport ke menu utama...",
    });

    setAuthToken(res.token);
  },
  onError: (err: ApiErrorDto) => {
    toast({
      title: "Waduh ada error",
      description: err.title,
    });
  },
});

const onSubmit = form.handleSubmit((formValues: LoginUserDto) => {
  loginMutate(formValues);
});
</script>

<template>
  <div class="flex justify-center items-center h-screen">
    <div>
      <h4 class="scroll-m-20 text-xl font-bold tracking-tight">
        Yuk Login Dulu 🚀
      </h4>

      <p class="mt-2 text-gray-600">
        Nanti biar data nya tersinkronisasi<br />
        sesuai dengan akun kamu
      </p>

      <form @submit="onSubmit" class="mt-4">
        <div v-for="(inputItem, idx) in inputItems" :key="idx">
          <FormField v-slot="{ componentField }" :name="inputItem.name">
            <FormItem class="mt-2">
              <FormControl>
                <Input
                  :type="inputItem.type"
                  :placeholder="inputItem.placeholder"
                  v-bind="componentField"
                />
              </FormControl>
              <FormMessage class="mt-2" />
            </FormItem>
          </FormField>
        </div>

        <Button class="mt-4 w-full" type="submit" :disabled="isPending">
          <span v-if="isPending">Proses...</span>
          <span v-else>Gasss 👌</span>
        </Button>
      </form>

      <hr class="my-4" />

      <NuxtLink to="/daftar">
        <Button variant="outline" class="mt-4 w-full">
          Daftar Dulu Deh 😁
        </Button>
      </NuxtLink>
    </div>
  </div>
</template>
