import { fromFile } from 'hasha';
import { rename } from 'fs';
import { basename, dirname, join } from 'path';
import { handleError } from './error-handler';

export const addHash: (filePath: string) => void = (filePath: string) => {
  const minCssExtension = '.min.css';
  const fileName = basename(filePath, minCssExtension);

  fromFile(filePath, { algorithm: 'md5' })
    .then(hash => {
        const hashedFileName = `${fileName}.${hash.substr(0, 20)}${minCssExtension}`;
        const directoryPath = dirname(filePath);

        rename(filePath, join(directoryPath, hashedFileName), handleError);
    });
};
