import { createSignal, onCleanup, type Component } from 'solid-js';

const App: Component = () => {
    var [currentTime, setCurrentTime] = createSignal(new Date());
    var [h, setH] = createSignal(0);
    var [m, setM] = createSignal(0);
    var [s, setS] = createSignal(0);

    const setDate = (theDate: Date) => {
        setCurrentTime(theDate);
        setH(theDate.getHours());
        setM(theDate.getMinutes());
        setS(theDate.getSeconds());
    };

    const classOff = "maw-m-1 maw-w-[24px] maw-h-[24px] maw-border-2 maw-rounded-full maw-border-solid maw-border-neutralContent";
    const classOn = `${classOff} maw-bg-neutralContent`;

    const getCellClass = (value: number, compareBit: number, isTens: boolean) => {
        const position = isTens ? 1 : 0;
        const paddedValue = `0${value}`;  // either will be 0x or 0xx
        const digit = parseInt(paddedValue.charAt(paddedValue.length - 1 - position), 10);
        const isOn = (digit & compareBit) === compareBit;

        return isOn ? classOn : classOff;
    };

    const pad = (val: number) => String(val).padStart(2, "0");

    const interval = setInterval(() => setDate(new Date()), 300);

    onCleanup(() => clearInterval(interval));

    return (
        <div class="not-prose maw-font-sans maw-font-[48px] maw-font-bold maw-flex maw-justify-center">
            <table class="maw-my-4 maw-mx-auto maw-border-separate maw-border-spacing-[4px]">
                <tbody>
                    <tr>
                        <td class={ getCellClass(h(), 8, true) }></td>
                        <td class={ getCellClass(h(), 8, false) }></td>
                        <td class="maw-w-4"></td>
                        <td class={ getCellClass(m(), 8, true) }></td>
                        <td class={ getCellClass(m(), 8, false) }></td>
                        <td class="maw-w-4"></td>
                        <td class={ getCellClass(s(), 8, true) }></td>
                        <td class={ getCellClass(s(), 8, false) }></td>
                    </tr>
                    <tr>
                        <td class={ getCellClass(h(), 4, true) }></td>
                        <td class={ getCellClass(h(), 4, false) }></td>
                        <td class="maw-w-4"></td>
                        <td class={ getCellClass(m(), 4, true) }></td>
                        <td class={ getCellClass(m(), 4, false) }></td>
                        <td class="maw-w-4"></td>
                        <td class={ getCellClass(s(), 4, true) }></td>
                        <td class={ getCellClass(s(), 4, false) }></td>
                    </tr>
                    <tr>
                        <td class={ getCellClass(h(), 2, true) }></td>
                        <td class={ getCellClass(h(), 2, false) }></td>
                        <td class="maw-w-4"></td>
                        <td class={ getCellClass(m(), 2, true) }></td>
                        <td class={ getCellClass(m(), 2, false) }></td>
                        <td class="maw-w-4"></td>
                        <td class={ getCellClass(s(), 2, true) }></td>
                        <td class={ getCellClass(s(), 2, false) }></td>
                    </tr>
                    <tr>
                        <td class={ getCellClass(h(), 1, true) }></td>
                        <td class={ getCellClass(h(), 1, false) }></td>
                        <td class="maw-w-4"></td>
                        <td class={ getCellClass(m(), 1, true) }></td>
                        <td class={ getCellClass(m(), 1, false) }></td>
                        <td class="maw-w-4"></td>
                        <td class={ getCellClass(s(), 1, true) }></td>
                        <td class={ getCellClass(s(), 1, false) }></td>
                    </tr>
                    <tr>
                        <td colspan="8" class="maw-text-center maw-b-0">
                            {pad(currentTime().getHours())} : {pad(currentTime().getMinutes())} : {pad(currentTime().getSeconds())}
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    );
};

export default App;
