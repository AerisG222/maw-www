import { createSignal, For, onCleanup, type Component } from 'solid-js';

const App: Component = () => {
    let audio: HTMLAudioElement;
    let intervalId: number | undefined;
    const [char, setChar] = createSignal("");
    const [currentSpeaker, setCurrentSpeaker] = createSignal("Mommy");
    const [currentLesson, setCurrentLesson] = createSignal("Letters");
    const [runButtonText, setRunButtonText] = createSignal("Run");
    const [doRun, setDoRun] = createSignal(false);

    const letters: string[] = [
        'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
        'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
    ];
    const numbers: string[] = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '10'];
    const combined: string[] = [...letters, ...numbers];
    const speakers: string[] = ['Mommy', 'Daddy', 'No Sound'];
    const lessons: string[] = ['Letters', 'Numbers', 'Random'];

    onCleanup(() => {
        if(intervalId) {
            clearInterval(intervalId);
        }
    });

    const toggleRunning = (): void => {
        setDoRun(!doRun());

        if (doRun()) {
            run();  // immediately start first character
            intervalId = setInterval(() => run(), 3000);  // schedule additional letters
        } else {
            if (intervalId) {
                clearInterval(intervalId);
            }
        }

        setRunButtonText(getButtonText());
    }

    const run = (): void => {
        setChar(getNextChar());

        if (currentSpeaker() !== 'No Sound') {
            audio.setAttribute("src", getCurrentAudioUrl());
            audio.load();
        }
    }

    const getCurrentAudioUrl = (): string => `/${currentSpeaker().toLowerCase()}/${char().toLowerCase()}.mp3`;

    const getButtonText = (): string => doRun() ? 'Stop' : 'Start';

    const getNextChar = (): string => {
        switch(currentLesson()) {
            case "Letters":
                return getNextInArray(char(), letters);
            case "Numbers":
                return getNextInArray(char(), numbers);
            default:
                return combined[Math.floor(Math.random() * combined.length)];
        }
    }

    const getNextInArray = (currItem: string, arr: string[]): string => {
        let i = arr.indexOf(currItem) + 1;

        if (i < 0 || i >= arr.length) {
            i = 0;
        }

        return arr[i];
    }

    const getDisplayChar = (char: string): string => {
        if (char === "") {
            return "&nbsp;";
        }

        return char;
    };

    return (
        <div class="maw-flex maw-flex-col maw-items-center maw-justify-center maw-font-sans">
            <audio autoplay ref={audio!}></audio>

            <div class="maw-w-[256px] maw-border-solid maw-border-2 maw-rounded-2xl maw-mb-8 maw-text-center">
                <span class="maw-font-brand maw-text-[128px]" innerHTML={getDisplayChar(char())}></span>
            </div>

            <div class="maw-flex maw-gap-16 maw-justify-center">
                <div class="maw-flex maw-flex-col">
                    <strong class="maw-mb-4">Speaker:</strong>
                    <For each={speakers}>{ (speaker) =>
                        <div class="maw-form-control">
                            <label class="maw-label maw-py-0 maw-cursor-pointer">
                                <span class="maw-label-text">{speaker}</span>
                                <input
                                    type="radio"
                                    name="speaker"
                                    class="maw-ml-2 maw-radio checked:maw-bg-primary"
                                    checked={currentSpeaker() === speaker}
                                    onChange={() => setCurrentSpeaker(speaker)} />
                            </label>
                        </div>
                    }</For>
                </div>

                <div class="maw-flex maw-flex-col">
                    <strong class="maw-mb-4">Lesson:</strong>
                    <For each={lessons}>{ (lesson) =>
                        <div class="maw-form-control">
                            <label class="maw-label maw-py-0 maw-cursor-pointer">
                                <span class="maw-label-text">{lesson}</span>
                                <input
                                    type="radio"
                                    name="lesson"
                                    class="maw-ml-2 maw-radio checked:maw-bg-primary"
                                    checked={currentLesson() === lesson}
                                    onChange={() => setCurrentLesson(lesson)} />
                            </label>
                        </div>
                    }</For>
                </div>
            </div>

            <div class="maw-m-8">
                <button
                    type="submit"
                    class="maw-btn maw-btn-primary"
                    onClick={toggleRunning}
                >
                    { runButtonText() }
                </button>
            </div>
        </div>
    );
};

export default App;
