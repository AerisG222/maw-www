import { createSignal, For, onSettled, type Component } from 'solid-js';

// hmm - maw:bg-<semantic-color> not working, so use gray for now
const CLASS_OFF = "maw:m-1 maw:w-[24px] maw:h-[24px] maw:border-2 maw:rounded-full maw:border-solid maw:transition";
const CLASS_ON = `${CLASS_OFF} maw:bg-base-content`;
const CLASS_SPACER = "maw:w-4";

// one row per bit, most significant first
const BITS = [8, 4, 2, 1];

const getCellClass = (value: number, compareBit: number, isTens: boolean) => {
    const position = isTens ? 1 : 0;
    const paddedValue = `0${value}`;  // either will be 0x or 0xx
    const digit = parseInt(paddedValue.charAt(paddedValue.length - 1 - position), 10);
    const isOn = (digit & compareBit) === compareBit;

    return isOn ? CLASS_ON : CLASS_OFF;
};

const pad = (val: number) => String(val).padStart(2, "0");

const App: Component = () => {
    const [currentTime, setCurrentTime] = createSignal(new Date());

    const timeParts = () => {
        const d = currentTime();

        return [d.getHours(), d.getMinutes(), d.getSeconds()];
    };

    // a row is the tens and ones cell of each time part, with a spacer between parts
    const rowClasses = (bit: number) =>
        timeParts().flatMap((value, i) => [
            ...(i > 0 ? [CLASS_SPACER] : []),
            getCellClass(value, bit, true),
            getCellClass(value, bit, false)
        ]);

    onSettled(() => {
        const interval = setInterval(() => setCurrentTime(new Date()), 300);

        return () => clearInterval(interval);
    });

    return (
        <div class="not-prose maw:font-sans maw:font-bold maw:flex maw:justify-center">
            <table class="maw:my-4 maw:mx-auto maw:border-separate maw:border-spacing-[4px]">
                <tbody>
                    <For each={BITS}>{ bit =>
                        <tr>
                            <For each={rowClasses(bit)} keyed={false}>{ cellClass =>
                                <td class={cellClass()}></td>
                            }</For>
                        </tr>
                    }</For>
                    <tr>
                        <td colspan="8" class="maw:text-center maw:b-0">
                            {pad(currentTime().getHours())} : {pad(currentTime().getMinutes())} : {pad(currentTime().getSeconds())}
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    );
};

export default App;
