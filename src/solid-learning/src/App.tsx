import { createSignal, For, onCleanup, type Component } from 'solid-js';

import styles from './App.module.css';

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

    return (
        <div class={styles.App}>
            <audio autoplay ref={audio!}></audio>

            <div class={styles["learning-letter-container"]}>
                <span class={styles["learning-letter"]}>{ char() }</span>
            </div>

            <div class={styles["option-container-wrapper"]}>
                <div class={styles["option-container"]}>
                    <strong>Speaker:</strong>
                    <For each={speakers}>{ (speaker) =>
                        <label class="form-check-label">
                            <input class="form-check-input" type="radio" name="speaker" checked={currentSpeaker() === speaker} onChange={() => setCurrentSpeaker(speaker)} />
                            {speaker}
                        </label>
                    }</For>
                </div>

                <div class={styles["option-container"]}>
                    <strong>Lesson:</strong>
                    <For each={lessons}>{ (lesson) =>
                        <label class="form-check-label">
                            <input class="form-check-input" type="radio" name="lesson" checked={currentLesson() === lesson} onChange={() => setCurrentLesson(lesson)} />
                            {lesson}
                        </label>
                    }</For>
                </div>
            </div>

            <div class={styles["action-container"]}>
                <button type="submit" onClick={toggleRunning}>{ runButtonText() }</button>
            </div>
        </div>
    );
};

export default App;
