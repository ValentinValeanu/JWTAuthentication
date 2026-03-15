import Image from "next/image";
import Menu from "./navigation/menu";
import { Geist, Geist_Mono } from "next/font/google";

const geistSans = Geist({
  variable: "--font-geist-sans",
  subsets: ["latin"],
});

const geistMono = Geist_Mono({
  variable: "--font-geist-mono",
  subsets: ["latin"],
});

export default function Home() {
  return (
    <div
      className={`${geistSans.className} ${geistMono.className} flex min-h-screen items-center justify-center bg-zinc-50 font-sans dark:bg-black`}
    >
      <main className="flex flex-col min-h-screen w-full max-w-3xl items-center py-32 px-5 bg-white dark:bg-black sm:items-start">
        <div className="flex w-full items-center justify-between">
          <Image
            className="dark:invert"
            src="/next.svg"
            alt="Next.js logo"
            width={100}
            height={20}
            priority
          />

          <Menu />
        </div>

        <div className="flex flex-1 items-center justify-center">
          <h1 className="text-center text-3xl font-semibold leading-10 tracking-tight text-black dark:text-zinc-50">
            Log-in to get started.
          </h1>
        </div>
      </main>
    </div>
  );
}
