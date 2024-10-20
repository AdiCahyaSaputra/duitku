<script setup lang="ts">
import FormTransaction from "@/components/reusable/ocr/form/FormTransaction.vue";
import { parseOCRText } from '@/lib/helper';
import { createWorker } from "tesseract.js";
import VuePictureCropper, { cropper } from "vue-picture-cropper";

definePageMeta({
  layout: "main-default",
});

useHead({
  title: "OCR (Eksperimental)",
});

const uploadInput = ref<HTMLInputElement | null>(null)
const pic = ref("");
const processOcrLoading = ref(false);

const dateRef = ref<null | Date>(null);
const amountRef = ref<number>(0);

const selectFile = (e: Event) => {
  pic.value = ''

  const { files } = e.target as HTMLInputElement
  if (!files || !files.length) return

  const file = files[0]
  const reader = new FileReader()
  reader.readAsDataURL(file)
  reader.onload = () => {
    pic.value = String(reader.result)

    if (!uploadInput.value) return;

    uploadInput.value.value = ''
  }
}

const processOcr = async () => {
  if(!cropper) return;

  processOcrLoading.value = true;

  const base64 = cropper.getDataURL();

  const worker = await createWorker('ind');
  const result = await worker.recognize(base64);

  processOcrLoading.value = false;

  const { date, amount, debug } = parseOCRText(result.data.text);

  dateRef.value = date;
  amountRef.value = +((amount as string).replaceAll(".", ""));

  await worker.terminate();
}
</script>

<template>
  <div class="w-full pb-20">
    <div class="flex gap-2 p-4 items-start border-b w-full">
      <NuxtLink to="/beranda">
        <Button size="icon" variant="outline">
          <Icon name="lucide:chevron-left" />
        </Button>
      </NuxtLink>
      <div>
        <h1 class="text-base font-bold">OCR (Eksperimental)</h1>
        <p class="text-xs">Input transaksi pake ss an / gambar</p>
      </div>
    </div>

    <div class="p-4 flex md:flex-row flex-col items-start gap-4">
      <div class="w-max flex flex-col md:items-start items-center">
        <div
          class="w-64 aspect-square bg-white p-4 border-4 border-emerald-600 border-dashed"
        >
          <VuePictureCropper
            :boxStyle="{
              width: '100%',
              height: '100%',
              backgroundColor: '#f8f8f8',
              margin: 'auto',
            }"
            :img="pic"
            :options="{
              viewMode: 1,
              dragMode: 'move',
              aspectRatio: 1,
              cropBoxResizable: true,
            }"
            :presetMode="{
              mode: 'fixedSize',
              width: 1000,
              height: 1000,
            }"
          />
        </div>

        <div class="mt-4 w-full">
          <div class="flex gap-2">
            <Button class="p-0" variant="outline">
              <label
                for="picture"
                class="py-2 px-4 text-sm font-medium  cursor-pointer"
              >
                🎦 Dari Galeri
              </label>
            </Button>
            <input
              type="file"
              class="hidden"
              id="picture"
              ref="uploadInput"
              accept="image/jpg, image/jpeg, image/png, image/gif"
              @change="selectFile"
            />
            <Button 
              @click="processOcr"
              class="flex items-center justify-start w-max gap-2" 
              :disabled="!pic.length || processOcrLoading"
            >
              <Icon name="lucide:scan-text"/>
              <span>{{ processOcrLoading ? 'Lagi ngubah...' : 'Ubah Ke Text' }}</span>
            </Button>
          </div>
        </div>
      </div>
      <div>
        <FormTransaction 
          :date="dateRef ?? new Date()"
          :amount="amountRef"
        />
      </div>
    </div>
  </div>
</template>
