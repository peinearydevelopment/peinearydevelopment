import { existsSync, mkdirSync, writeFile } from 'fs';
import { render, Result, SassError } from 'node-sass';
import { argv } from 'process';
import { handleError } from './error-handler';
import { addHash } from './add-hash-to-filename';

const isProductionBuild: boolean = !!argv.find((arg: string) => arg === 'production' || arg === 'prod' || arg === '-P' || arg === '--prod');

const outputDir = `../wwwroot`;

if (!existsSync(outputDir)) {
    mkdirSync(outputDir);
}

const inputFilePath = `./styles/layout.scss`;
const cssOutputFilePath = `${outputDir}/site.min.css`;
const cssMapOutputFilePath = `${outputDir}/site.css.map`;

render({
    file: inputFilePath,
    outFile: cssOutputFilePath,
    outputStyle: isProductionBuild ? 'compressed' : 'expanded',
    sourceMap: !isProductionBuild
},
(sassError: SassError, result: Result): void => {
    handleError(sassError);

    writeFile(cssOutputFilePath, result.css, handleError);
    if (!isProductionBuild) {
        writeFile(cssMapOutputFilePath, result.map, handleError);
    }

    addHash(cssOutputFilePath);
});
