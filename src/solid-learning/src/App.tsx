import { createSignal, For, onCleanup, type Component } from 'solid-js';

const LETTERS: string[] = [
    'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
    'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
];
const NUMBERS: string[] = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '10'];
const COMBINED: string[] = [...LETTERS, ...NUMBERS];
const SPEAKERS: string[] = ['Mommy', 'Daddy', 'No Sound'];
const LESSONS: string[] = ['Letters', 'Numbers', 'Random'];

const NBSP = "\u00A0";  // keeps the display box from collapsing before the first character

const getNextInArray = (currItem: string, arr: string[]): string => {
    let i = arr.indexOf(currItem) + 1;

    if (i < 0 || i >= arr.length) {
        i = 0;
    }

    return arr[i];
};

const App: Component = () => {
    let audio!: HTMLAudioElement;
    let intervalId: ReturnType<typeof setInterval> | undefined;
    const [char, setChar] = createSignal("");
    const [currentSpeaker, setCurrentSpeaker] = createSignal("Mommy");
    const [currentLesson, setCurrentLesson] = createSignal("Letters");
    const [isRunning, setIsRunning] = createSignal(false);

    const runButtonText = () => isRunning() ? "Stop" : "Start";
    const displayChar = () => char() === "" ? NBSP : char();
    const currentAudioUrl = () => `${import.meta.env.VITE_ASSET_ROOT}${currentSpeaker().toLowerCase()}/${char().toLowerCase()}.mp3`;

    const getNextChar = (): string => {
        switch(currentLesson()) {
            case "Letters":
                return getNextInArray(char(), LETTERS);
            case "Numbers":
                return getNextInArray(char(), NUMBERS);
            default:
                return COMBINED[Math.floor(Math.random() * COMBINED.length)];
        }
    };

    // playing a sound is a one-off action, not derived state, so it stays imperative -
    // deriving it from `char` would go silent when the same character repeats
    const run = (): void => {
        setChar(getNextChar());

        if (currentSpeaker() !== 'No Sound') {
            audio.src = currentAudioUrl();
            audio.load();
        }
    };

    const stopRunning = (): void => {
        clearInterval(intervalId);
        intervalId = undefined;
        setIsRunning(false);
    };

    const startRunning = (): void => {
        setIsRunning(true);
        run();  // immediately start first character
        intervalId = setInterval(run, 3000);  // schedule additional letters
    };

    const toggleRunning = (): void => isRunning() ? stopRunning() : startRunning();

    onCleanup(stopRunning);

    return (
        <div class="maw:flex maw:flex-col maw:items-center maw:justify-center maw:font-sans">
            <audio autoplay ref={audio}></audio>

            <div class="maw:w-[256px] maw:border-solid maw:border-2 maw:rounded-2xl maw:mb-8 maw:text-center">
                <span class="maw:font-inter maw:text-[128px]">{displayChar()}</span>
            </div>

            <div class="maw:flex maw:gap-16 maw:justify-center">
                <div class="maw:flex maw:flex-col">
                    <strong class="maw:mb-4">Speaker:</strong>
                    <For each={SPEAKERS}>{ (speaker) =>
                        <div class="maw:form-control maw:mb-2">
                            <label class="maw:label maw:py-0 maw:cursor-pointer">
                                <span class="maw:label-text">{speaker}</span>
                                <input
                                    type="radio"
                                    name="speaker"
                                    class="maw:ml-2 maw:radio checked:maw:bg-primary"
                                    checked={currentSpeaker() === speaker}
                                    onChange={() => setCurrentSpeaker(speaker)} />
                            </label>
                        </div>
                    }</For>
                </div>

                <div class="maw:flex maw:flex-col">
                    <strong class="maw:mb-4">Lesson:</strong>
                    <For each={LESSONS}>{ (lesson) =>
                        <div class="maw:form-control maw:mb-2">
                            <label class="maw:label maw:py-0 maw:cursor-pointer">
                                <span class="maw:label-text">{lesson}</span>
                                <input
                                    type="radio"
                                    name="lesson"
                                    class="maw:ml-2 maw:radio checked:maw:bg-primary"
                                    checked={currentLesson() === lesson}
                                    onChange={() => setCurrentLesson(lesson)} />
                            </label>
                        </div>
                    }</For>
                </div>
            </div>

            <div class="maw:m-6">
                <button
                    type="submit"
                    class="maw:btn maw:btn-primary"
                    onClick={toggleRunning}
                >
                    { runButtonText() }
                </button>
            </div>
        </div>
    );
};

export default App;
