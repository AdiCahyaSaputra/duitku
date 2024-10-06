<script setup lang="ts">
import { toTypedSchema } from "@vee-validate/zod";
import * as z from "zod";
import { useForm } from "vee-validate";
import { useMutation } from "@tanstack/vue-query";
import { registerUser } from "@/services/auth-service";
import type RegisterUserDto from "@/dto/RegisterUserDto";
import { useToast } from "@/components/ui/toast/use-toast";

definePageMeta({
  layout: "default",
});

useHead({
  title: "Daftar",
});

const inputItems: { name: string; placeholder: string; type: string }[] = [
  {
    name: "name",
    placeholder: "Nama Lengkap",
    type: "text",
  },
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
    name: z
      .string({ message: "Nama Lengkap harus di isi" })
      .trim()
      .min(1, "Nama lengkap harus di isi"),
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

const {
  isPending,
  isError,
  mutate: registerMutate,
} = useMutation({
  mutationFn: registerUser,
  onSuccess: (res) => {
    toast({
      title: "Notifikasi",
      description: "Teleport ke menu utama...",
    });
  },
  onError: (err) => {
    toast({
      title: "Waduh ada error",
      description: "Ada yang salah dari sisi server wkwkwk",
    });
  },
});

const onSubmit = form.handleSubmit((formValues: RegisterUserDto) => {
  registerMutate(formValues);
});
</script>

<template>
  <div class="flex justify-center items-center h-screen">
    <div>
      <h4 class="scroll-m-20 text-xl font-bold tracking-tight">
        Yaudh Daftar Dulu 😁
      </h4>

      <p class="mt-2 text-gray-600">
        Nanti biar data nya bisa di collect<br />
        eh maksud nya di backup ke akun ini
      </p>

      <form @submit="onSubmit" class="mt-4">
        <div v-for="(inputItem, idx) in inputItems" :key="idx">
          <FormField v-slot="{ componentField }" :name="inputItem.name">
            <FormItem class="mt-2">
              <FormControl>
                <Input
                  :type="inputItem.type"
                  :placeholder="inputItem.placeholder"
                  :disabled="isPending"
                  v-bind="componentField"
                />
              </FormControl>
              <FormMessage class="mt-2" />
            </FormItem>
          </FormField>
        </div>

        <Button class="mt-4 w-full" type="submit" :disabled="isPending">
          <span v-if="isPending">Proses...</span>
          <span v-else>Udah Bener Nih 👍</span>
        </Button>
      </form>

      <hr class="my-4" />

      <NuxtLink to="/login">
        <Button variant="outline" class="mt-4 w-full" :disabled="isPending">
          Udah Pernah Daftar? 🤔
        </Button>
      </NuxtLink>
    </div>
  </div>
</template>
