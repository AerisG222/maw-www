import { createSignal, onCleanup, type Component } from 'solid-js';

import styles from './App.module.css';

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
    }

    const getCellClass = (value: number, compareBit: number, isTens: boolean) => {
        const position = isTens ? 1 : 0;
        const paddedValue = `0${value}`;  // either will be 0x or 0xx
        const digit = parseInt(paddedValue.charAt(paddedValue.length - 1 - position), 10);

        if ((digit & compareBit) === compareBit) {
            return styles.bc_on;
        } else {
            return styles.bc_off;
        }
    }

    const interval = setInterval(() => setDate(new Date()), 300);

    onCleanup(() => clearInterval(interval));

    return (
        <div class={styles.App}>
            <table class={styles.binclock}>
                <tbody>
                    <tr>
                        <td class={ getCellClass(h(), 8, true) }></td>
                        <td class={ getCellClass(h(), 8, false) }></td>
                        <td class={ styles.spacer }></td>
                        <td class={ getCellClass(m(), 8, true) }></td>
                        <td class={ getCellClass(m(), 8, false) }></td>
                        <td class={ styles.spacer }></td>
                        <td class={ getCellClass(s(), 8, true) }></td>
                        <td class={ getCellClass(s(), 8, false) }></td>
                    </tr>
                    <tr>
                        <td class={ getCellClass(h(), 4, true) }></td>
                        <td class={ getCellClass(h(), 4, false) }></td>
                        <td class={ styles.spacer }></td>
                        <td class={ getCellClass(m(), 4, true) }></td>
                        <td class={ getCellClass(m(), 4, false) }></td>
                        <td class={ styles.spacer }></td>
                        <td class={ getCellClass(s(), 4, true) }></td>
                        <td class={ getCellClass(s(), 4, false) }></td>
                    </tr>
                    <tr>
                        <td class={ getCellClass(h(), 2, true) }></td>
                        <td class={ getCellClass(h(), 2, false) }></td>
                        <td class={ styles.spacer }></td>
                        <td class={ getCellClass(m(), 2, true) }></td>
                        <td class={ getCellClass(m(), 2, false) }></td>
                        <td class={ styles.spacer }></td>
                        <td class={ getCellClass(s(), 2, true) }></td>
                        <td class={ getCellClass(s(), 2, false) }></td>
                    </tr>
                    <tr>
                        <td class={ getCellClass(h(), 1, true) }></td>
                        <td class={ getCellClass(h(), 1, false) }></td>
                        <td class={ styles.spacer }></td>
                        <td class={ getCellClass(m(), 1, true) }></td>
                        <td class={ getCellClass(m(), 1, false) }></td>
                        <td class={ styles.spacer }></td>
                        <td class={ getCellClass(s(), 1, true) }></td>
                        <td class={ getCellClass(s(), 1, false) }></td>
                    </tr>
                    <tr>
                        <td colspan="8" class={ styles.center }>
                            {currentTime().getHours()} : {currentTime().getMinutes()} : {currentTime().getSeconds()}
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    );
};

export default App;
