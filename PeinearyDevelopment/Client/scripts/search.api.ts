export class SearchApi {
  private typewatch = (() => {
      let timer = 0;
      return (callback: () => void, ms: number) => {
          clearTimeout(timer);
          timer = setTimeout(callback, ms);
      };
  })();

  constructor() {
    document.addEventListener('DOMContentLoaded', event => this.registerSearchListener(), false);
  }

  public registerSearchListener() {
      const searchboxInput = document.getElementById('searchbox') as HTMLInputElement;
      searchboxInput.addEventListener('keydown', event => {
                 this.typewatch(() => this.search(searchboxInput.value), 500);
             });
  }

  public search(searchTerm: string) {
      fetch(`/blog/search?searchTerm=${searchTerm}`)
          .then(response => response.text())
          .then(html => console.log(html));
  }
}
