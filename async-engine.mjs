import { Marp } from "@marp-team/marp-core";
import marpMermaid, { postProcessor } from "./marp-mermaid-plugin.mjs";

/*
 * Custom Marp engine with async post-processing
 * Useful for async rendering
 * https://github.com/markdown-it/markdown-it/issues/248
 */
class PostprocessMarpitEngine extends Marp {
  constructor(options, postprocess) {
    super(options);
    this.postprocess = postprocess;
  }

  withPostprocess(postprocess) {
    this.postprocess = postprocess;
    return this;
  }

  async render(markdown, env = {}) {
    const { html, css, comments } = super.render(markdown, env);
    if (this.postprocess) {
      return await this.postprocess(markdown, env, html, css, comments);
    }
    return { html, css, comments };
  }
}

export default async (constructorOptions) => {
  return new PostprocessMarpitEngine(constructorOptions)
    .use(marpMermaid)
    .withPostprocess(postProcessor);
};